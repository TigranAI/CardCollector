using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class EditGiveawayStructure2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_channel_giveaways_telegram_chats_channel_id",
                table: "channel_giveaways");

            migrationBuilder.AlterColumn<long>(
                name: "channel_id",
                table: "channel_giveaways",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "fk_channel_giveaways_telegram_chats_channel_id",
                table: "channel_giveaways",
                column: "channel_id",
                principalTable: "telegram_chats",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_channel_giveaways_telegram_chats_channel_id",
                table: "channel_giveaways");

            migrationBuilder.AlterColumn<long>(
                name: "channel_id",
                table: "channel_giveaways",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_channel_giveaways_telegram_chats_channel_id",
                table: "channel_giveaways",
                column: "channel_id",
                principalTable: "telegram_chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
