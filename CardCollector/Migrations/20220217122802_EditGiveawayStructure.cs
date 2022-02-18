using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class EditGiveawayStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "selected_sticker_id",
                table: "channel_giveaways",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_channel_giveaways_selected_sticker_id",
                table: "channel_giveaways",
                column: "selected_sticker_id");

            migrationBuilder.AddForeignKey(
                name: "fk_channel_giveaways_stickers_selected_sticker_id",
                table: "channel_giveaways",
                column: "selected_sticker_id",
                principalTable: "stickers",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_channel_giveaways_stickers_selected_sticker_id",
                table: "channel_giveaways");

            migrationBuilder.DropIndex(
                name: "ix_channel_giveaways_selected_sticker_id",
                table: "channel_giveaways");

            migrationBuilder.DropColumn(
                name: "selected_sticker_id",
                table: "channel_giveaways");
        }
    }
}
