using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class EditUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_telegram_chats_telegram_chat_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_telegram_chat_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "telegram_chat_id",
                table: "users");

            migrationBuilder.CreateTable(
                name: "telegram_chat_user",
                columns: table => new
                {
                    available_chats_id = table.Column<long>(type: "bigint", nullable: false),
                    members_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_telegram_chat_user", x => new { x.available_chats_id, x.members_id });
                    table.ForeignKey(
                        name: "fk_telegram_chat_user_telegram_chats_available_chats_id",
                        column: x => x.available_chats_id,
                        principalTable: "telegram_chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_telegram_chat_user_users_members_id",
                        column: x => x.members_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_telegram_chat_user_members_id",
                table: "telegram_chat_user",
                column: "members_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_chat_user");

            migrationBuilder.AddColumn<long>(
                name: "telegram_chat_id",
                table: "users",
                type: "bigint",
                nullable: true);

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
    }
}
