using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AuthTokens_Users_UserId",
            //    table: "AuthTokens");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Users_Users_CreatedById",
            //    table: "Users");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Users_Users_UpdatedById",
            //    table: "Users");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Users",
            //    table: "Users");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_AuthTokens",
            //    table: "AuthTokens");

            //migrationBuilder.RenameTable(
            //    name: "Users",
            //    newName: "User");

            //migrationBuilder.RenameTable(
            //    name: "AuthTokens",
            //    newName: "AuthToken");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Users_UpdatedById",
            //    table: "User",
            //    newName: "IX_User_UpdatedById");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Users_CreatedById",
            //    table: "User",
            //    newName: "IX_User_CreatedById");

            //migrationBuilder.RenameIndex(
            //    name: "IX_AuthTokens_UserId",
            //    table: "AuthToken",
            //    newName: "IX_AuthToken_UserId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_User",
            //    table: "User",
            //    column: "Id");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_AuthToken",
            //    table: "AuthToken",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_AuthToken_User_UserId",
            //    table: "AuthToken",
            //    column: "UserId",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_User_User_CreatedById",
            //    table: "User",
            //    column: "CreatedById",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_User_User_UpdatedById",
            //    table: "User",
            //    column: "UpdatedById",
            //    principalTable: "User",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthToken_User_UserId",
                table: "AuthToken");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedById",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedById",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthToken",
                table: "AuthToken");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "AuthToken",
                newName: "AuthTokens");

            migrationBuilder.RenameIndex(
                name: "IX_User_UpdatedById",
                table: "Users",
                newName: "IX_Users_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_User_CreatedById",
                table: "Users",
                newName: "IX_Users_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AuthToken_UserId",
                table: "AuthTokens",
                newName: "IX_AuthTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthTokens",
                table: "AuthTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthTokens_Users_UserId",
                table: "AuthTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatedById",
                table: "Users",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
