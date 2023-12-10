using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_02.Persistence.Entities;

namespace Task_02.Persistence.Configurations;

public class BankClassConfiguration : IEntityTypeConfiguration<BankClass>
{
    public void Configure(EntityTypeBuilder<BankClass> builder)
    {
        builder.ToTable("BankClasses");
        
        builder.HasKey(x => new { x.BankId, x.ClassNumber });

        builder.Property(x => x.Name).IsUnicode().IsRequired();

        builder
            .HasOne(x => x.Bank)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}