using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class EditCountableLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_activities_users_user_id",
                table: "user_activities");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "user_activities",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "additional_data",
                table: "user_activities",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "group_count_was_active",
                table: "count_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "group_count_was_added",
                table: "count_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "group_prize_count",
                table: "count_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "roulette_play_count",
                table: "count_logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_user_activities_users_user_id",
                table: "user_activities",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_activities_users_user_id",
                table: "user_activities");

            migrationBuilder.DropColumn(
                name: "additional_data",
                table: "user_activities");

            migrationBuilder.DropColumn(
                name: "group_count_was_active",
                table: "count_logs");

            migrationBuilder.DropColumn(
                name: "group_count_was_added",
                table: "count_logs");

            migrationBuilder.DropColumn(
                name: "group_prize_count",
                table: "count_logs");

            migrationBuilder.DropColumn(
                name: "roulette_play_count",
                table: "count_logs");

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "user_activities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_user_activities_users_user_id",
                table: "user_activities",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
