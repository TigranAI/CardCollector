using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class UserStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users_stats",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    earned_exp = table.Column<long>(type: "bigint", nullable: false),
                    last_exp_accrual = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    earned4tier_stickers = table.Column<int>(type: "int", nullable: false),
                    last4tier_sticker_accrual = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    roulette_games = table.Column<int>(type: "int", nullable: false),
                    last_roulette_game = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ladder_games = table.Column<int>(type: "int", nullable: false),
                    last_ladder_game = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    puzzle_games = table.Column<int>(type: "int", nullable: false),
                    last_puzzle_game = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    gifts_received = table.Column<int>(type: "int", nullable: false),
                    last_received_gift = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    friends_invited = table.Column<int>(type: "int", nullable: false),
                    last_invited_friend = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_stats_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users_stats");
        }
    }
}
