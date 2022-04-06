using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class BeginnersTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_buy_standard_pack",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_buy_sticker_on_auction",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tasks_progress_claim_income",
                table: "invite_info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_combine_stickers",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_invite_friend",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_open_pack",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_place_sticker_on_auction",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tasks_progress_play_roulette",
                table: "invite_info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tasks_progress_send_stickers_to_private",
                table: "invite_info",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "tasks_progress_take_part_at_chat_giveaway",
                table: "invite_info",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tasks_progress_win_roulette",
                table: "invite_info",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tasks_progress_buy_standard_pack",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_buy_sticker_on_auction",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_claim_income",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_combine_stickers",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_invite_friend",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_open_pack",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_place_sticker_on_auction",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_play_roulette",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_send_stickers_to_private",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_take_part_at_chat_giveaway",
                table: "invite_info");

            migrationBuilder.DropColumn(
                name: "tasks_progress_win_roulette",
                table: "invite_info");
        }
    }
}
