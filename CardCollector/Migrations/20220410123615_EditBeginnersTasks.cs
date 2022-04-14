using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class EditBeginnersTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "claim_income",
                table: "beginners_tasks_progress",
                newName: "collect_income");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "collect_income",
                table: "beginners_tasks_progress",
                newName: "claim_income");
        }
    }
}
