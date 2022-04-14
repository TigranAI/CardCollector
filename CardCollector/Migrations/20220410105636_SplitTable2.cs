using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class SplitTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "beginners_tasks_progress",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    progress = table.Column<int>(type: "int", nullable: false),
                    send_stickers_to_private = table.Column<int>(type: "int", nullable: false),
                    combine_stickers = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    buy_sticker_on_auction = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    buy_standard_pack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    open_pack = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    take_part_at_chat_giveaway = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    claim_income = table.Column<int>(type: "int", nullable: false),
                    play_roulette = table.Column<int>(type: "int", nullable: false),
                    win_roulette = table.Column<int>(type: "int", nullable: false),
                    place_sticker_on_auction = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    invite_friend = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    donate = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_beginners_tasks_progress", x => x.id);
                    table.ForeignKey(
                        name: "fk_beginners_tasks_progress_invite_info_id",
                        column: x => x.id,
                        principalTable: "invite_info",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beginners_tasks_progress");

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
    }
}
