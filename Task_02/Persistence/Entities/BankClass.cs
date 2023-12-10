using Task_02.Persistence.Entities.Base;

namespace Task_02.Persistence.Entities;

public class BankClass : Entity
{
    public byte ClassNumber { get; set; }

    public string Name { get; set; } = null!;
    
    public int BankId { get; set; }
    
    public Bank Bank { get; set; } = null!;
}