using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_02.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankClasses",
                columns: table => new
                {
                    ClassNumber = table.Column<byte>(type: "tinyint", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankClasses", x => new { x.BankId, x.ClassNumber });
                    table.ForeignKey(
                        name: "FK_BankClasses_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TurnoverStatements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnoverStatements", x => x.Id);
                    table.CheckConstraint("CK_TurnoverStatements_PeriodStartBeforePeriodEnd", "PeriodStart < PeriodEnd");
                    table.ForeignKey(
                        name: "FK_TurnoverStatements_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    AccountNumber = table.Column<short>(type: "smallint", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ClassNumber = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => new { x.BankId, x.AccountNumber });
                    table.ForeignKey(
                        name: "FK_BankAccounts_BankClasses_BankId_ClassNumber",
                        columns: x => new { x.BankId, x.ClassNumber },
                        principalTable: "BankClasses",
                        principalColumns: new[] { "BankId", "ClassNumber" });
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccountTurnoverStatements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<short>(type: "smallint", nullable: false),
                    TurnoverStatementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IncomingBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DebitTurnover = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreditTurnover = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTurnoverStatements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountTurnoverStatements_BankAccounts_BankId_AccountNumber",
                        columns: x => new { x.BankId, x.AccountNumber },
                        principalTable: "BankAccounts",
                        principalColumns: new[] { "BankId", "AccountNumber" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTurnoverStatements_TurnoverStatements_TurnoverStatementId",
                        column: x => x.TurnoverStatementId,
                        principalTable: "TurnoverStatements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTurnoverStatements_BankId_AccountNumber",
                table: "AccountTurnoverStatements",
                columns: new[] { "BankId", "AccountNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTurnoverStatements_TurnoverStatementId",
                table: "AccountTurnoverStatements",
                column: "TurnoverStatementId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId_ClassNumber",
                table: "BankAccounts",
                columns: new[] { "BankId", "ClassNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_Name",
                table: "Banks",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TurnoverStatements_BankId",
                table: "TurnoverStatements",
                column: "BankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTurnoverStatements");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "TurnoverStatements");

            migrationBuilder.DropTable(
                name: "BankClasses");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
