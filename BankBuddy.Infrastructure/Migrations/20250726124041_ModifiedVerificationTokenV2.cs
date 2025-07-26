using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankBuddy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedVerificationTokenV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
