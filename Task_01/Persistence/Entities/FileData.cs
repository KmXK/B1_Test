using Task_01.Persistence.Entities.Base;

namespace Task_01.Persistence.Entities;

public class FileData : Entity<Guid>
{
    public DateOnly Date { get; set; }

    public string RussianString { get; set; } = null!;

    public string EnglishString { get; set; } = null!;
    
    public int EvenInteger { get; set; }
    
    public double RandomDouble { get; set; }
}