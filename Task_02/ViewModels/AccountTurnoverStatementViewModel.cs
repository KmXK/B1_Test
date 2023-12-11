using System.Globalization;
using Task_02.Persistence.Entities;
using Task_02.Persistence.Enums;

namespace Task_02.ViewModels;

public class AccountTurnoverStatementViewModel
{
    private static readonly NumberFormatInfo NumberFormatInfo = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = " "
    };

    public AccountTurnoverStatementViewModel(AccountTurnoverStatement accountTurnoverStatement)
    {
        AccountNumber = accountTurnoverStatement.AccountNumber.ToString();
        DebitTurnover = accountTurnoverStatement.DebitTurnover;
        CreditTurnover = accountTurnoverStatement.CreditTurnover;
        IsGroup = false;
        ActiveIncoming = GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            true,
            accountTurnoverStatement.IncomingBalance);
        PassiveIncoming = GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            false,
            accountTurnoverStatement.IncomingBalance);
        ActiveOutgoing = GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            true,
            accountTurnoverStatement.OutgoingBalance);
        PassiveOutgoing = GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            false,
            accountTurnoverStatement.OutgoingBalance);
    }

    public AccountTurnoverStatementViewModel(string groupName)
    {
        AccountNumber = groupName;
        ActiveIncoming = 0;
        PassiveIncoming = 0;
        DebitTurnover = 0;
        CreditTurnover = 0;
        ActiveOutgoing = 0;
        PassiveOutgoing = 0;
        IsGroup = true;
    }

    public string AccountNumber { get; }

    public decimal ActiveIncoming { get; private set; }

    public decimal PassiveIncoming { get; private set; }

    public decimal DebitTurnover { get; private set; }

    public decimal CreditTurnover { get; private set; }

    public decimal ActiveOutgoing { get; private set; }
    
    public decimal PassiveOutgoing { get; private set; }
    
    public bool IsGroup { get; }
    
    public string ClassName { get; set; }

    public void Add(AccountTurnoverStatementViewModel viewModel)
    {
        ActiveIncoming += viewModel.ActiveIncoming;
        PassiveIncoming += viewModel.PassiveIncoming;
        DebitTurnover += viewModel.DebitTurnover;
        CreditTurnover += viewModel.CreditTurnover;
        ActiveOutgoing += viewModel.ActiveOutgoing;
        PassiveOutgoing += viewModel.PassiveOutgoing;
    }

    private static decimal GetActivePassiveValue(AccountType type, bool isActive, decimal value)
    {
        if (isActive && type == AccountType.Active || !isActive && type == AccountType.Passive)
        {
            return value;
        }

        return 0;
    }

    private static string FormatDecimal(decimal value)
    {
        return value.ToString("n", NumberFormatInfo);
    }
}