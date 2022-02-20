using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class ModifyUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_users_channel_giveaways_channel_giveaway_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_channel_giveaway_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "channel_giveaway_id",
                table: "users");

            migrationBuilder.CreateTable(
                name: "channel_giveaway_user",
                columns: table => new
                {
                    awarded_users_id = table.Column<long>(type: "bigint", nullable: false),
                    used_giveaways_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_channel_giveaway_user", x => new { x.awarded_users_id, x.used_giveaways_id });
                    table.ForeignKey(
                        name: "fk_channel_giveaway_user_channel_giveaways_used_giveaways_id",
                        column: x => x.used_giveaways_id,
                        principalTable: "channel_giveaways",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_channel_giveaway_user_users_awarded_users_id",
                        column: x => x.awarded_users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_channel_giveaway_user_used_giveaways_id",
                table: "channel_giveaway_user",
                column: "used_giveaways_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "channel_giveaway_user");

            migrationBuilder.AddColumn<int>(
                name: "channel_giveaway_id",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_channel_giveaway_id",
                table: "users",
                column: "channel_giveaway_id");

            migrationBuilder.AddForeignKey(
                name: "fk_users_channel_giveaways_channel_giveaway_id",
                table: "users",
                column: "channel_giveaway_id",
                principalTable: "channel_giveaways",
                principalColumn: "id");
        }
    }
}
