using System.ComponentModel.DataAnnotations;

namespace Task_02.Persistence.Entities.Base;

public class Entity<TKey> : Entity
{
    [Key]
    public TKey Id { get; set; } = default!;
}