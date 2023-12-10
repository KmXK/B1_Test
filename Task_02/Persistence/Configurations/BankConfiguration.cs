using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_02.Persistence.Entities;

namespace Task_02.Persistence.Configurations;

public class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks");
        
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name);

        builder.Property(x => x.Name).IsUnicode().HasMaxLength(100);
    }
}