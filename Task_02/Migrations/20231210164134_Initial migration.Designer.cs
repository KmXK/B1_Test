﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Task_02.Persistence;

#nullable disable

namespace Task_02.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231210164134_Initial migration")]
    partial class Initialmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<long>("CreditTurnover")
                        .HasColumnType("bigint");

                    b.Property<long>("DebitTurnover")
                        .HasColumnType("bigint");

                    b.Property<long>("IncomingBalance")
                        .HasColumnType("bigint");

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

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("BankId", "AccountNumber");

                    b.ToTable("BankAccounts", (string)null);
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankClass", b =>
                {
                    b.Property<int>("BankId")
                        .HasColumnType("int");

                    b.Property<byte>("ClassNumber")
                        .HasColumnType("tinyint");

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
                        .WithMany()
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

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Task_02.Persistence.Entities.BankClass", b =>
                {
                    b.HasOne("Task_02.Persistence.Entities.Bank", "Bank")
                        .WithMany()
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
#pragma warning restore 612, 618
        }
    }
}
