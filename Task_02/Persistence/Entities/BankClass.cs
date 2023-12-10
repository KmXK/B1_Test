using Task_02.Persistence.Entities.Base;

namespace Task_02.Persistence.Entities;

public class BankClass : Entity
{
    public byte ClassNumber { get; set; }
    
    public int BankId { get; set; }
    
    public Bank Bank { get; set; } = null!;
}