using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class ExclusiveStickerTasksGoal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "exclusive_task_progress",
                table: "user_stickers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "exclusive_task_goal",
                table: "stickers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exclusive_task_progress",
                table: "user_stickers");

            migrationBuilder.DropColumn(
                name: "exclusive_task_goal",
                table: "stickers");
        }
    }
}
