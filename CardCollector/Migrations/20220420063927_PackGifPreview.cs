using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class PackGifPreview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "gif_preview_file_id",
                table: "packs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gif_preview_file_id",
                table: "packs");
        }
    }
}
