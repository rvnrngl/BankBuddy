using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankBuddy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBankAccountProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "AccountStatus",
                table: "BankAccounts",
                type: "text",
                nullable: false,
                defaultValue: "Active",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountType",
                table: "BankAccounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "AccountStatus",
                table: "BankAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Active");
        }
    }
}
