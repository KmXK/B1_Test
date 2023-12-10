﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task_02.Persistence.Entities;

namespace Task_02.Persistence.Configurations;

public class BankClassConfiguration : IEntityTypeConfiguration<BankClass>
{
    public void Configure(EntityTypeBuilder<BankClass> builder)
    {
        builder.ToTable("BankClasses");
        
        builder.HasKey(x => new { x.BankId, x.ClassNumber });

        builder
            .HasOne(x => x.Bank)
            .WithMany()
            .HasForeignKey(x => x.BankId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}