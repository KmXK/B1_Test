using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task_01.Migrations
{
    /// <inheritdoc />
    public partial class Initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilesData",
                columns: table => new
                {
                    Key = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    RussianString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EvenInteger = table.Column<int>(type: "int", nullable: false),
                    RandomDouble = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilesData", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilesData");
        }
    }
}
