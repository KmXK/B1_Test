using System.Globalization;
using System.IO;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Task_02.Exceptions;
using Task_02.Models;
using Task_02.Persistence;
using Task_02.Persistence.Entities;
using Task_02.Persistence.Enums;
using Task_02.Services.Interfaces;

namespace Task_02.Services;

public class TurnoverExcelImporter(AppDbContext context) : ITurnoverExcelImporter
{
    static TurnoverExcelImporter()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }
   
    public Task<TurnoverStatement> ImportAsync(string fileName)
    {
        return Task.Run(() =>
        {
            using var package = new ExcelPackage(new FileInfo(fileName));

            var worksheet = package.Workbook.Worksheets[0];

            return ReadTurnoverStatement(worksheet);
        });
    }

    private static string ReadBankName(ExcelWorksheet worksheet)
    {
        var bankName =  worksheet.Cells["A1"].Text;

        if (string.IsNullOrWhiteSpace(bankName))
        {
            throw new ExcelParseException("Bank name not found.");
        }

        return bankName;
    }

    private static DateTime ReadTurnoverCreationDate(ExcelWorksheet worksheet)
    {
        var value = worksheet.Cells["A6:B6"].Merge
            ? worksheet.Cells["A6:B6"] 
            : worksheet.Cells["A6"];

        if (string.IsNullOrWhiteSpace(value.Text))
        {
            throw new ExcelParseException("Turnover creation date not found.");
        }
        
        if (DateTime.TryParseExact(
                value.Text,
                "d.M.yyyy H:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date))
        {
            return date;
        }
        
        throw new ExcelParseException("Invalid turnover creation date format.");
    }

    private static (DateTime From, DateTime To) ReadTurnoverPeriod(ExcelWorksheet worksheet)
    {
        var value = worksheet.Cells["A3:G3"];

        if (value.Merge == false || string.IsNullOrWhiteSpace(value.Text))
        {
            throw new ExcelParseException("Turnover period not found.");
        }
        
        // for format "за период с dd.MM.yyyy по dd.MM.yyyy"
        var values = value.Text[12..].Split(" по ");
        
        if (TryParseDate(values[0], out var from) &&
            TryParseDate(values[1], out var to))
        {
            return (from, to);
        }
        
        throw new ExcelParseException("Invalid turnover period date format.");

        bool TryParseDate(string stringValue, out DateTime date)
        {
            return DateTime.TryParseExact(
                stringValue,
                "dd.MM.yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);
        }
    }

    private TurnoverStatement ReadTurnoverStatement(ExcelWorksheet worksheet)
    {
        var readClasses = ReadClasses(worksheet);
        var bankName = ReadBankName(worksheet);
        var turnoverCreationDate = ReadTurnoverCreationDate(worksheet);
        var (from, to) = ReadTurnoverPeriod(worksheet);
        
        using var transaction = context.Database.BeginTransaction();
        
        var bank = context
            .Set<Bank>()
            .Include(x => x.Classes)
            .FirstOrDefault(x => x.Name == bankName);
        
        if (bank == null)
        {
            bank = new Bank
            {
                Name = bankName
            };

            context.Add(bank);
            context.SaveChanges();
        }
        
        var existingAccounts = context
            .Set<BankAccount>()
            .Where(x => x.BankId == bank.Id)
            .Select(x => new { x.AccountNumber, x.ClassNumber, x.Type })
            .ToDictionary(x => x.AccountNumber);

        var classes = bank.Classes.ToDictionary(x => x.ClassNumber);
        var accountStatements = new List<AccountTurnoverStatement>();
        
        foreach (var (classNumber, className, classAccountStatements) in readClasses)
        {
            // If bank class with such number already exists.
            if (classes.TryGetValue(classNumber, out var bankClass))
            {
                // Check if class name is the same.
                if (bankClass.Name != className)
                {
                    throw new ExcelParseException($"Class with number {classNumber} already exists and has different name.");
                }
            }
            else
            {
                bankClass = new BankClass
                {
                    BankId = bank.Id,
                    Bank = bank,
                    ClassNumber = classNumber,
                    Name = className
                };
                
                bank.Classes.Add(bankClass);
            }
            
            // If bank already contains classes.
            if (classes.Count > 0)
            {
                // Check if there are accounts with different class number.
                var accountsWithDifferentClassAmount = classAccountStatements
                    .Count(x => existingAccounts.TryGetValue(x.AccountNumber, out var existingAccount) &&
                                existingAccount.ClassNumber != bankClass.ClassNumber);

                if (accountsWithDifferentClassAmount > 0)
                {
                    throw new ExcelParseException(
                        $"{accountsWithDifferentClassAmount} existing accounts with different class number were found.");
                }
            }

            foreach (var accountStatement in classAccountStatements)
            {
                // Update account data.
                accountStatement.Account.AccountNumber = accountStatement.AccountNumber;
                accountStatement.Account.BankId = bank.Id;
                accountStatement.Account.ClassNumber = bankClass.ClassNumber;
                accountStatement.Account.Bank = bank;
                accountStatement.Account.BankClass = bankClass;
                
                // If account already exists
                if (existingAccounts.TryGetValue(accountStatement.AccountNumber, out var existingAccount))
                {
                    // Check his type and throw exception if it's different.
                    if (existingAccount.Type != AccountType.Undefined &&
                        existingAccount.Type != accountStatement.Account.Type)
                    {
                        throw new ExcelParseException(
                            $"Account with number {accountStatement.AccountNumber} already exists and has different type.");
                    }

                    // Attach entity to context to update it.
                    context.Attach(accountStatement.Account);
                }
               
                accountStatements.Add(accountStatement);
            }
        }

        var statement = new TurnoverStatement
        {
            BankId = bank.Id,
            Bank = bank,
            CreationDate = turnoverCreationDate,
            PeriodStart = from,
            PeriodEnd = to,
            AccountTurnoverStatements = accountStatements
        };

        context.Add(statement);
        context.SaveChanges();
        
        transaction.Commit();

        return statement;
    }

    private static List<BankClassParsingResult> ReadClasses(ExcelWorksheet worksheet)
    {
        // Row number for first class
        var row = 9;

        var result = new List<BankClassParsingResult>();

        while (ReadClass(worksheet, ref row) is { } classParsingResult)
        {
            result.Add(classParsingResult);
        }

        return result;
    }
    
    private static BankClassParsingResult? ReadClass(
        ExcelWorksheet worksheet,
        ref int row)
    {
        var header = worksheet.Cells[row, 1, row, 7];

        if (header.Merge == false || header.Text[..5] != "КЛАСС")
        {
            return null;
        }

        var parts = header.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var classNumber = byte.Parse(parts[1]);
        var className = string.Join(' ', parts[2..]);
            
        row++;
        
        var accountTurnoverStatements = new List<AccountTurnoverStatement>();
            
        while (short.TryParse(worksheet.Cells[row, 1].Text, out var value))
        {
            if (value < 100)
            {
                // Skip group row for 00xx
                row++;
                continue;
            }

            var accountStatement = ReadAccountTurnoverStatement(worksheet, row);

            accountStatement.AccountNumber = value;
            
            accountTurnoverStatements.Add(accountStatement);
                
            row++;
        }

        // Skip group row for class
        row++;

        return new BankClassParsingResult(classNumber, className, accountTurnoverStatements);
    }

    private static AccountTurnoverStatement ReadAccountTurnoverStatement(ExcelWorksheet worksheet, int row)
    {
        var activeIncome = decimal.Parse(worksheet.Cells[row, 2].Text);
        var passiveIncome = decimal.Parse(worksheet.Cells[row, 3].Text);
        var debitTurnover = decimal.Parse(worksheet.Cells[row, 4].Text);
        var creditTurnover = decimal.Parse(worksheet.Cells[row, 5].Text);
        var activeOutgoing = decimal.Parse(worksheet.Cells[row, 6].Text);
        var passiveOutgoing = decimal.Parse(worksheet.Cells[row, 7].Text);

        var statement = new AccountTurnoverStatement
        {
            IncomingBalance = activeIncome == 0 ? passiveIncome : activeIncome,
            DebitTurnover = debitTurnover,
            CreditTurnover = creditTurnover,
            Account = new BankAccount
            {
                Type = activeIncome != 0 && passiveIncome == 0
                    ? AccountType.Active
                    : activeIncome == 0 && passiveIncome != 0 
                        ? AccountType.Passive
                        : AccountType.Undefined
            }
        };

        var outgoingBalance = activeIncome != 0 ? activeOutgoing : passiveOutgoing;

        if (statement.OutgoingBalance != outgoingBalance)
        {
            // TODO: Warning user about not valid value and (maybe) stop parsing
        }

        return statement;
    }
}