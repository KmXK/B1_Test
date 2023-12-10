using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_02.Persistence.Entities;

namespace Task_02.Persistence.Configurations;

public class TurnoverStatementConfiguration : IEntityTypeConfiguration<TurnoverStatement>
{
    public void Configure(EntityTypeBuilder<TurnoverStatement> builder)
    {
        builder.ToTable("TurnoverStatements", b =>
        {
            b.HasCheckConstraint(
                "CK_TurnoverStatements_PeriodStartBeforePeriodEnd",
                "PeriodStart < PeriodEnd");
        });
        
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Bank)
            .WithMany()
            .HasForeignKey(x => x.BankId);
    }
}