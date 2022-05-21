using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class RefactorUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "user_cash");

            migrationBuilder.DropTable(
                name: "user_level");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.CreateTable(
                name: "cash",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    coins = table.Column<int>(type: "int", nullable: false),
                    gems = table.Column<int>(type: "int", nullable: false),
                    candies = table.Column<int>(type: "int", nullable: false),
                    max_capacity = table.Column<int>(type: "int", nullable: false),
                    last_payout = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cash", x => x.id);
                    table.ForeignKey(
                        name: "fk_cash_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    settings = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_settings_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_levels",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    current_exp = table.Column<long>(type: "bigint", nullable: false),
                    total_exp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_levels", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_levels_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropTable(
                name: "cash");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "user_levels");

            migrationBuilder.CreateTable(
                name: "user_cash",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    candies = table.Column<int>(type: "int", nullable: false),
                    coins = table.Column<int>(type: "int", nullable: false),
                    gems = table.Column<int>(type: "int", nullable: false),
                    last_payout = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    max_capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cash", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_cash_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_level",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    current_exp = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    total_exp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_level", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_level_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    settings = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_settings_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");*/
        }
    }
}
