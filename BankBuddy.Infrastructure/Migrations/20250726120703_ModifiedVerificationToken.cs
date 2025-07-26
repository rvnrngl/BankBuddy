using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankBuddy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedVerificationToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationToken_Users_UserId",
                table: "VerificationToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationToken",
                table: "VerificationToken");

            migrationBuilder.RenameTable(
                name: "VerificationToken",
                newName: "VerificationTokens");

            migrationBuilder.RenameIndex(
                name: "IX_VerificationToken_UserId",
                table: "VerificationTokens",
                newName: "IX_VerificationTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationTokens",
                table: "VerificationTokens",
                column: "VerificationTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VerificationTokens_Users_UserId",
                table: "VerificationTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VerificationTokens",
                table: "VerificationTokens");

            migrationBuilder.RenameTable(
                name: "VerificationTokens",
                newName: "VerificationToken");

            migrationBuilder.RenameIndex(
                name: "IX_VerificationTokens_UserId",
                table: "VerificationToken",
                newName: "IX_VerificationToken_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VerificationToken",
                table: "VerificationToken",
                column: "VerificationTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_VerificationToken_Users_UserId",
                table: "VerificationToken",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
