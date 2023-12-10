using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_02.Persistence.Entities;

namespace Task_02.Persistence.Configurations;

public class AccountTurnoverStatementConfiguration : IEntityTypeConfiguration<AccountTurnoverStatement>
{
    public void Configure(EntityTypeBuilder<AccountTurnoverStatement> builder)
    {
        builder.ToTable("AccountTurnoverStatements");
        
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Account)
            .WithMany()
            .HasForeignKey(x => new { x.BankId, x.AccountNumber });

        builder
            .HasOne(x => x.TurnoverStatement)
            .WithMany()
            .HasForeignKey(x => x.TurnoverStatementId);
        
        builder.Ignore(x => x.OutgoingBalance);
    }
}