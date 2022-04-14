using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class DisableDistributions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "distributions_disabled",
                table: "telegram_chats",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "distributions_disabled",
                table: "telegram_chats");
        }
    }
}
