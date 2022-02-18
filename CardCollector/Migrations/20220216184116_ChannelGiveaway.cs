using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class ChannelGiveaway : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "channel_giveaway_id",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "channel_giveaways",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    prize = table.Column<int>(type: "int", nullable: false),
                    total_count = table.Column<int>(type: "int", nullable: false),
                    claimable_prize_at_interval = table.Column<int>(type: "int", nullable: false),
                    interval_minutes = table.Column<int>(type: "int", nullable: false),
                    channel_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_channel_giveaways", x => x.id);
                    table.ForeignKey(
                        name: "fk_channel_giveaways_telegram_chats_channel_id",
                        column: x => x.channel_id,
                        principalTable: "telegram_chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_users_channel_giveaway_id",
                table: "users",
                column: "channel_giveaway_id");

            migrationBuilder.CreateIndex(
                name: "ix_channel_giveaways_channel_id",
                table: "channel_giveaways",
                column: "channel_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_channel_giveaways_channel_giveaway_id",
                table: "users",
                column: "channel_giveaway_id",
                principalTable: "channel_giveaways",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_channel_giveaways_channel_giveaway_id",
                table: "users");

            migrationBuilder.DropTable(
                name: "channel_giveaways");

            migrationBuilder.DropIndex(
                name: "ix_users_channel_giveaway_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "channel_giveaway_id",
                table: "users");
        }
    }
}
