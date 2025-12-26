using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RewardFlow_API.Migrations.EmployeeDb
{
    /// <inheritdoc />
    public partial class AddEmployeeHashedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "national_number",
                schema: "dbo",
                table: "employees",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "account_number_hash",
                schema: "dbo",
                table: "employees",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "national_number_hash",
                schema: "dbo",
                table: "employees",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CreatedBy_AccountNumberHash",
                schema: "dbo",
                table: "employees",
                columns: new[] { "created_by", "account_number_hash" },
                unique: true,
                filter: "[account_number_hash] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CreatedBy_NationalNumberHash",
                schema: "dbo",
                table: "employees",
                columns: new[] { "created_by", "national_number_hash" },
                unique: true,
                filter: "[national_number_hash] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employee_CreatedBy_AccountNumberHash",
                schema: "dbo",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_Employee_CreatedBy_NationalNumberHash",
                schema: "dbo",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "account_number_hash",
                schema: "dbo",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "national_number_hash",
                schema: "dbo",
                table: "employees");

            migrationBuilder.AlterColumn<string>(
                name: "national_number",
                schema: "dbo",
                table: "employees",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
