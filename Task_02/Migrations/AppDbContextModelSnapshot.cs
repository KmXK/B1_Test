﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task_02.Persistence;

#nullable disable

namespace Task_02.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Task_02.Persistence.Entities.AccountTurnoverStatement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("AccountNumber")
                        .HasColumnType("smallint");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<decimal>("CreditTurnover")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DebitTurnover")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("IncomingBalance")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("TurnoverStatementId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TurnoverStatementId");

                    b.HasIndex("BankId", "AccountNumber");

                    b.ToTable("AccountTurnoverStatements", (string)null);
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Banks", (string)null);
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankAccount", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<short>("AccountNumber")
                        .HasColumnType("smallint");

                    b.Property<int>("BankClassBankId")
                        .HasColumnType("int");

                    b.Property<byte>("BankClassClassNumber")
                        .HasColumnType("tinyint");

                    b.Property<byte>("ClassNumber")
                        .HasColumnType("tinyint");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("BankId", "AccountNumber");

                    b.HasIndex("BankClassBankId", "BankClassClassNumber");

                    b.ToTable("BankAccounts", (string)null);
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankClass", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<byte>("ClassNumber")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BankId", "ClassNumber");

                    b.ToTable("BankClasses", (string)null);
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.TurnoverStatement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodStart")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("TurnoverStatements", null, t =>
                        {
                            t.HasCheckConstraint("CK_TurnoverStatements_PeriodStartBeforePeriodEnd", "PeriodStart < PeriodEnd");
                        });
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.AccountTurnoverStatement", b =>
                {
                    b.HasOne("Task_02.Persistence.Entities.TurnoverStatement", "TurnoverStatement")
                        .WithMany("AccountTurnoverStatements")
                        .HasForeignKey("TurnoverStatementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Task_02.Persistence.Entities.BankAccount", "Account")
                        .WithMany()
                        .HasForeignKey("BankId", "AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("TurnoverStatement");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankAccount", b =>
                {
                    b.HasOne("Task_02.Persistence.Entities.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Task_02.Persistence.Entities.BankClass", "BankClass")
                        .WithMany()
                        .HasForeignKey("BankClassBankId", "BankClassClassNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("BankClass");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankClass", b =>
                {
                    b.HasOne("Task_02.Persistence.Entities.Bank", "Bank")
                        .WithMany("Classes")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.TurnoverStatement", b =>
                {
                    b.HasOne("Task_02.Persistence.Entities.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.Bank", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.TurnoverStatement", b =>
                {
                    b.Navigation("AccountTurnoverStatements");
                });
#pragma warning restore 612, 618
        }
    }
}
