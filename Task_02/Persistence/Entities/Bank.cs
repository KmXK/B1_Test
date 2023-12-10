using Task_02.Persistence.Entities.Base;

namespace Task_02.Persistence.Entities;

public class Bank : Entity<int>
{
    public string Name { get; set; } = null!;

    public ICollection<BankClass> Classes { get; set; } = new HashSet<BankClass>();
}