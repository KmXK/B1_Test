using Task_02.Persistence.Entities.Base;
using Task_02.Persistence.Enums;

namespace Task_02.Persistence.Entities;

public class BankAccount : Entity
{
    public AccountType Type { get; set; }
    
    public short AccountNumber { get; set; }
    
    public int BankId { get; set; }

    public Bank Bank { get; set; } = null!;
}