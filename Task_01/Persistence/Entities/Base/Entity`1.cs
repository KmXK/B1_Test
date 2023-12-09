using System.ComponentModel.DataAnnotations;

namespace Task_01.Persistence.Entities.Base;

public class Entity<TKey> : Entity
{
    [Key]
    public TKey Key { get; set; } = default!;
}