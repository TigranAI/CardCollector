using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class ExclusivePacks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_unlocked",
                table: "user_stickers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "exclusive_task",
                table: "stickers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "gray_file_id",
                table: "stickers",
                type: "varchar(127)",
                maxLength: 127,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "is_exclusive",
                table: "packs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_unlocked",
                table: "user_stickers");

            migrationBuilder.DropColumn(
                name: "exclusive_task",
                table: "stickers");

            migrationBuilder.DropColumn(
                name: "gray_file_id",
                table: "stickers");

            migrationBuilder.DropColumn(
                name: "is_exclusive",
                table: "packs");
        }
    }
}
