using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPswEntryCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordEntry_User_UserId",
                table: "PasswordEntry");

            migrationBuilder.DropIndex(
                name: "IX_PasswordEntry_UserId",
                table: "PasswordEntry");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PasswordEntry");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "PasswordEntry",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PasswordEntry",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "PasswordEntry",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "PasswordEntry",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "PasswordEntry",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordEntry_UserId",
                table: "PasswordEntry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordEntry_User_UserId",
                table: "PasswordEntry",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
