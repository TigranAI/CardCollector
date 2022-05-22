using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class TopHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "fk_cash_users_id",
                table: "cash");

            migrationBuilder.DropForeignKey(
                name: "fk_settings_users_id",
                table: "settings");

            migrationBuilder.DropForeignKey(
                name: "fk_user_levels_users_id",
                table: "user_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_levels",
                table: "user_levels");

            migrationBuilder.DropPrimaryKey(
                name: "pk_settings",
                table: "settings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_cash",
                table: "cash");

            migrationBuilder.RenameTable(
                name: "user_levels",
                newName: "user_level");

            migrationBuilder.RenameTable(
                name: "settings",
                newName: "user_settings");

            migrationBuilder.RenameTable(
                name: "cash",
                newName: "user_cash");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_level",
                table: "user_level",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_settings",
                table: "user_settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_cash",
                table: "user_cash",
                column: "id");*/

            migrationBuilder.CreateTable(
                name: "top_history",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    exp = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tier4 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    roulette = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ladder = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    puzzle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gift = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    invite = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_top_history", x => x.date);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            /*migrationBuilder.AddForeignKey(
                name: "fk_user_cash_users_id",
                table: "user_cash",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_level_users_id",
                table: "user_level",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_settings_users_id",
                table: "user_settings",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "fk_user_cash_users_id",
                table: "user_cash");

            migrationBuilder.DropForeignKey(
                name: "fk_user_level_users_id",
                table: "user_level");

            migrationBuilder.DropForeignKey(
                name: "fk_user_settings_users_id",
                table: "user_settings");*/

            migrationBuilder.DropTable(
                name: "top_history");

            /*migrationBuilder.DropPrimaryKey(
                name: "pk_user_settings",
                table: "user_settings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_level",
                table: "user_level");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_cash",
                table: "user_cash");

            migrationBuilder.RenameTable(
                name: "user_settings",
                newName: "settings");

            migrationBuilder.RenameTable(
                name: "user_level",
                newName: "user_levels");

            migrationBuilder.RenameTable(
                name: "user_cash",
                newName: "cash");

            migrationBuilder.AddPrimaryKey(
                name: "pk_settings",
                table: "settings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_levels",
                table: "user_levels",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_cash",
                table: "cash",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_cash_users_id",
                table: "cash",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_settings_users_id",
                table: "settings",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_user_levels_users_id",
                table: "user_levels",
                column: "id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
