using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PasswordManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Username",
            //    table: "Users",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<long>(
            //    name: "UpdatedById",
            //    table: "Users",
            //    type: "bigint",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "UpdatedAt",
            //    table: "Users",
            //    type: "timestamp with time zone",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "PasswordHash",
            //    table: "Users",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<string>(
            //    name: "LastName",
            //    table: "Users",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "LastLoginAt",
            //    table: "Users",
            //    type: "timestamp with time zone",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<bool>(
            //    name: "IsActive",
            //    table: "Users",
            //    type: "boolean",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<string>(
            //    name: "FirstName",
            //    table: "Users",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Email",
            //    table: "Users",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<long>(
            //    name: "CreatedById",
            //    table: "Users",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    table: "Users",
            //    type: "timestamp with time zone",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<int>(
            //    name: "BadPwdCount",
            //    table: "Users",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Id",
            //    table: "Users",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER")
            //    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            //migrationBuilder.AlterColumn<long>(
            //    name: "UserId",
            //    table: "AuthTokens",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            //migrationBuilder.AlterColumn<string>(
            //    name: "Token",
            //    table: "AuthTokens",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "RevokedAt",
            //    table: "AuthTokens",
            //    type: "timestamp with time zone",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "ExpiresAt",
            //    table: "AuthTokens",
            //    type: "timestamp with time zone",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<string>(
            //    name: "DeviceInfo",
            //    table: "AuthTokens",
            //    type: "text",
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "CreatedAt",
            //    table: "AuthTokens",
            //    type: "timestamp with time zone",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "TEXT");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Id",
            //    table: "AuthTokens",
            //    type: "bigint",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER")
            //    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "Users",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedAt",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastLoginAt",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IsActive",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "BadPwdCount",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "AuthTokens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "AuthTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "RevokedAt",
                table: "AuthTokens",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpiresAt",
                table: "AuthTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceInfo",
                table: "AuthTokens",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "AuthTokens",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AuthTokens",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
