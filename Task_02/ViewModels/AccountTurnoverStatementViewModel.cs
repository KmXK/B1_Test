using System.Globalization;
using Task_02.Persistence.Entities;
using Task_02.Persistence.Enums;

namespace Task_02.ViewModels;

public class AccountTurnoverStatementViewModel(AccountTurnoverStatement accountTurnoverStatement)
{
    private static readonly NumberFormatInfo NumberFormatInfo = new NumberFormatInfo
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = " "
    };
    
    public string AccountNumber { get; set; } = accountTurnoverStatement.AccountNumber.ToString();

    public string ActiveIncoming { get; set; } =
        GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            true,
            accountTurnoverStatement.IncomingBalance);

    public string PassiveIncoming { get; set; } =
        GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            false,
            accountTurnoverStatement.IncomingBalance);

    public string DebitTurnover { get; set; } = FormatDecimal(accountTurnoverStatement.DebitTurnover);

    public string CreditTurnover { get; set; } = FormatDecimal(accountTurnoverStatement.CreditTurnover);

    public string ActiveOutgoing { get; set; } = 
        GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            true,
            accountTurnoverStatement.OutgoingBalance);
    
    public string PassiveOutgoing { get; set; } = 
        GetActivePassiveValue(
            accountTurnoverStatement.Account.Type,
            false,
            accountTurnoverStatement.OutgoingBalance);

    private static string GetActivePassiveValue(AccountType type, bool isActive, decimal value)
    {
        if (isActive && type == AccountType.Active || !isActive && type == AccountType.Passive)
        {
            return FormatDecimal(value);
        }

        return FormatDecimal(0);
    }

    private static string FormatDecimal(decimal value)
    {
        return value.ToString("n", NumberFormatInfo);
    }
}