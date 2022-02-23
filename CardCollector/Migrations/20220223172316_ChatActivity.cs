using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class ChatActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "telegram_chat_id",
                table: "users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "chat_activity",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    message_count = table.Column<long>(type: "bigint", nullable: false),
                    last_giveaway = table.Column<DateTime>(type: "datetime(6)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "ix_users_telegram_chat_id",
                table: "users",
                column: "telegram_chat_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_telegram_chats_telegram_chat_id",
                table: "users",
                column: "telegram_chat_id",
                principalTable: "telegram_chats",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_telegram_chats_telegram_chat_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "chat_activity");

            migrationBuilder.DropIndex(
                name: "ix_users_telegram_chat_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "telegram_chat_id",
                table: "users");
        }
    }
}
