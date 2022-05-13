using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class RemoveChatActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_activity");

            migrationBuilder.AddColumn<int>(
                name: "candies",
                table: "user_cash",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_giveaway",
                table: "telegram_chats",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "members_count",
                table: "telegram_chats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "income_type",
                table: "stickers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "candies",
                table: "user_cash");

            migrationBuilder.DropColumn(
                name: "last_giveaway",
                table: "telegram_chats");

            migrationBuilder.DropColumn(
                name: "members_count",
                table: "telegram_chats");

            migrationBuilder.DropColumn(
                name: "income_type",
                table: "stickers");

            migrationBuilder.CreateTable(
                name: "chat_activity",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    giveaway_available = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    last_giveaway = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    message_count = table.Column<long>(type: "bigint", nullable: false),
                    message_count_at_last_giveaway = table.Column<long>(type: "bigint", nullable: false),
                    prize_claimed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_activity", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_activity_telegram_chats_id",
                        column: x => x.id,
                        principalTable: "telegram_chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
