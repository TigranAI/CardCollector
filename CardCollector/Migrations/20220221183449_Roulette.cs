using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class Roulette : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "chat_roulette_id",
                table: "user_stickers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "chat_roulette",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    creator_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<long>(type: "bigint", nullable: false),
                    message_id = table.Column<int>(type: "int", nullable: false),
                    is_started = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_roulette", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_roulette_telegram_chats_group_id",
                        column: x => x.group_id,
                        principalTable: "telegram_chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_roulette_users_creator_id",
                        column: x => x.creator_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_user_stickers_chat_roulette_id",
                table: "user_stickers",
                column: "chat_roulette_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_roulette_creator_id",
                table: "chat_roulette",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_roulette_group_id",
                table: "chat_roulette",
                column: "group_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_stickers_chat_roulette_chat_roulette_id",
                table: "user_stickers",
                column: "chat_roulette_id",
                principalTable: "chat_roulette",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_stickers_chat_roulette_chat_roulette_id",
                table: "user_stickers");

            migrationBuilder.DropTable(
                name: "chat_roulette");

            migrationBuilder.DropIndex(
                name: "ix_user_stickers_chat_roulette_id",
                table: "user_stickers");

            migrationBuilder.DropColumn(
                name: "chat_roulette_id",
                table: "user_stickers");
        }
    }
}
