using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RewardFlow_API.Migrations.RewardDb
{
    /// <inheritdoc />
    public partial class InitialCreate_RewardDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "rewards",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    total = table.Column<decimal>(type: "smallmoney", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    last_update = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    reward_type = table.Column<int>(type: "int", nullable: false),
                    number_of_employees = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rewards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    is_theoretical = table.Column<bool>(type: "bit", nullable: false),
                    is_practical = table.Column<bool>(type: "bit", nullable: false),
                    subject_price = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee_rewards",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    reward_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsUpdated = table.Column<bool>(type: "bit", nullable: false),
                    RewardEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_rewards", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_rewards_rewards_RewardEntityId",
                        column: x => x.RewardEntityId,
                        principalSchema: "dbo",
                        principalTable: "rewards",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "session_rewards",
                schema: "dbo",
                columns: table => new
                {
                    session_reward_id = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Semester = table.Column<byte>(type: "tinyint", nullable: true),
                    percentage = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session_rewards", x => x.session_reward_id);
                    table.ForeignKey(
                        name: "FK_session_rewards_rewards_session_reward_id",
                        column: x => x.session_reward_id,
                        principalSchema: "dbo",
                        principalTable: "rewards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject_semesters",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    semester_number = table.Column<byte>(type: "tinyint", nullable: false),
                    number_of_students = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_semesters", x => x.id);
                    table.ForeignKey(
                        name: "FK_subject_semesters_subjects_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "dbo",
                        principalTable: "subjects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subject_session_rewards",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    session_reward_id = table.Column<int>(type: "int", nullable: false),
                    number_of_sessions = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    students_number = table.Column<int>(type: "int", nullable: false),
                    main_employee_id = table.Column<int>(type: "int", nullable: true),
                    max_number_of_employees = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_session_rewards", x => x.id);
                    table.ForeignKey(
                        name: "FK_subject_session_rewards_subject_semesters_subject_id",
                        column: x => x.subject_id,
                        principalSchema: "dbo",
                        principalTable: "subject_semesters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_session_rewards",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_session_reward_id = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<int>(type: "int", nullable: false),
                    NumberOfSessions = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_session_rewards", x => x.id);
                    table.ForeignKey(
                        name: "FK_employee_session_rewards_subject_session_rewards_subject_session_reward_id",
                        column: x => x.subject_session_reward_id,
                        principalSchema: "dbo",
                        principalTable: "subject_session_rewards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee_rewards_RewardEntityId",
                schema: "dbo",
                table: "employee_rewards",
                column: "RewardEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_employee_session_rewards_subject_session_reward_id",
                schema: "dbo",
                table: "employee_session_rewards",
                column: "subject_session_reward_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_semesters_subject_id",
                schema: "dbo",
                table: "subject_semesters",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_subject_session_rewards_subject_id",
                schema: "dbo",
                table: "subject_session_rewards",
                column: "subject_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_rewards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "employee_session_rewards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "session_rewards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "subject_session_rewards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "rewards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "subject_semesters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "subjects",
                schema: "dbo");
        }
    }
}
