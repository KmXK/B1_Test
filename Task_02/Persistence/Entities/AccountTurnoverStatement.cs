using Task_02.Persistence.Entities.Base;
using Task_02.Persistence.Enums;

namespace Task_02.Persistence.Entities;

public class AccountTurnoverStatement : Entity<Guid>
{
    public int BankId { get; set; }
    
    public short AccountNumber { get; set; }
    
    public Guid TurnoverStatementId { get; set; }
    
    public long IncomingBalance { get; set; }
    
    public long DebitTurnover { get; set; }
    
    public long CreditTurnover { get; set; }
    
    public BankAccount Account { get; set; } = null!;

    public TurnoverStatement TurnoverStatement { get; set; } = null!;

    public long OutgoingBalance
    {
        get
        {
            if (IncomingBalance == 0) return 0;

            return Account.Type switch
            {
                AccountType.Active => IncomingBalance + DebitTurnover - CreditTurnover,
                AccountType.Passive => IncomingBalance - DebitTurnover + CreditTurnover,
                AccountType.Undefined => 0,
                _ => throw new ArgumentOutOfRangeException(nameof(Account.Type))
            };
        }
    }
}