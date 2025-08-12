using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reward_Flow_v2.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIntId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id",
                schema: "dbo",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_users_user_id",
                schema: "dbo",
                table: "users",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_user_id",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "id",
                schema: "dbo",
                table: "users");
        }
    }
}
