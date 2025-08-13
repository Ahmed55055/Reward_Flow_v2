using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reward_Flow_v2.Migrations.EmployeeDb
{
    /// <inheritdoc />
    public partial class UpdatingUserForeignKeyAddTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_employees_user_created_by",
                schema: "dbo",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_faculties_user_created_by",
                schema: "dbo",
                table: "faculties");

            migrationBuilder.DropIndex(
                name: "UQ_Faculties_CreatedBy_Name",
                schema: "dbo",
                table: "faculties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "dbo",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "dbo",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1")
                .Annotation("SqlServer:Clustered", true);

            // Add temporary columns for the new int foreign keys
            migrationBuilder.AddColumn<int>(
                name: "created_by_temp",
                schema: "dbo",
                table: "faculties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "created_by_temp",
                schema: "dbo",
                table: "employees",
                type: "int",
                nullable: true);

            // Update the temporary columns with the new int IDs
            migrationBuilder.Sql(@"
                UPDATE f SET f.created_by_temp = u.Id 
                FROM [dbo].[faculties] f 
                INNER JOIN [dbo].[users] u ON f.created_by = u.user_id
            ");

            migrationBuilder.Sql(@"
                UPDATE e SET e.created_by_temp = u.Id 
                FROM [dbo].[employees] e 
                INNER JOIN [dbo].[users] u ON e.created_by = u.user_id
            ");

            // Drop indexes before dropping columns
            migrationBuilder.DropIndex(
                name: "IX_employees_created_by",
                schema: "dbo",
                table: "employees");

            

            // Drop old columns
            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "dbo",
                table: "faculties");

            migrationBuilder.DropColumn(
                name: "created_by",
                schema: "dbo",
                table: "employees");

            // Rename temp columns to final names
            migrationBuilder.RenameColumn(
                name: "created_by_temp",
                schema: "dbo",
                table: "faculties",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "created_by_temp",
                schema: "dbo",
                table: "employees",
                newName: "created_by");

            // Make the columns non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "dbo",
                table: "faculties",
                type: "int",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                schema: "dbo",
                table: "employees",
                type: "int",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                schema: "dbo",
                table: "users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UQ_Faculties_CreatedBy_Name",
                schema: "dbo",
                table: "faculties",
                columns: new[] { "created_by", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_created_by",
                schema: "dbo",
                table: "employees",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_faculties_created_by",
                schema: "dbo",
                table: "faculties",
                column: "created_by");

            migrationBuilder.CreateTable(
                name: "employee_name_tokens",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token_hashed = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    n = table.Column<byte>(type: "tinyint", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_name_tokens", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_name_tokens_user_id_token_hashed",
                schema: "dbo",
                table: "employee_name_tokens",
                columns: new[] { "user_id", "token_hashed" });

            migrationBuilder.AddForeignKey(
                name: "FK_employees_user_created_by",
                schema: "dbo",
                table: "employees",
                column: "created_by",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_faculties_user_created_by",
                schema: "dbo",
                table: "faculties",
                column: "created_by",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employees_user_created_by",
                schema: "dbo",
                table: "employees");

            migrationBuilder.DropForeignKey(
                name: "FK_faculties_user_created_by",
                schema: "dbo",
                table: "faculties");

            migrationBuilder.DropTable(
                name: "employee_name_tokens",
                schema: "dbo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                schema: "dbo",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "dbo",
                table: "users");

            migrationBuilder.AlterColumn<Guid>(
                name: "created_by",
                schema: "dbo",
                table: "faculties",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "created_by",
                schema: "dbo",
                table: "employees",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                schema: "dbo",
                table: "users",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_employees_user_created_by",
                schema: "dbo",
                table: "employees",
                column: "created_by",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_faculties_user_created_by",
                schema: "dbo",
                table: "faculties",
                column: "created_by",
                principalSchema: "dbo",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
