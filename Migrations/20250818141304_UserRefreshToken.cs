using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reward_Flow_v2.Migrations
{
    /// <inheritdoc />
    public partial class UserRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                schema: "dbo",
                table: "users",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expiry",
                schema: "dbo",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_expiry",
                schema: "dbo",
                table: "users");
        }
    }
}
