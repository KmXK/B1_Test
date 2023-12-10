using Task_02.Persistence.Entities.Base;

namespace Task_02.Persistence.Entities;

public class TurnoverStatement : Entity<Guid>
{
    public int BankId { get; set; }
    
    public DateTime PeriodStart { get; set; }
    
    public DateTime PeriodEnd { get; set; }
    
    public DateTime CreationDate { get; set; }

    public Bank Bank { get; set; } = null!;
}