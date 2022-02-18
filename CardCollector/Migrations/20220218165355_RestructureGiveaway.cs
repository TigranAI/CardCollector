using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class RestructureGiveaway : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "claimable_prize_at_interval",
                table: "channel_giveaways");

            migrationBuilder.DropColumn(
                name: "interval_minutes",
                table: "channel_giveaways");

            migrationBuilder.RenameColumn(
                name: "total_count",
                table: "channel_giveaways",
                newName: "prize_count");

            migrationBuilder.AddColumn<string>(
                name: "button_text",
                table: "channel_giveaways",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_file_id",
                table: "channel_giveaways",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "message",
                table: "channel_giveaways",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "send_at",
                table: "channel_giveaways",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "button_text",
                table: "channel_giveaways");

            migrationBuilder.DropColumn(
                name: "image_file_id",
                table: "channel_giveaways");

            migrationBuilder.DropColumn(
                name: "message",
                table: "channel_giveaways");

            migrationBuilder.DropColumn(
                name: "send_at",
                table: "channel_giveaways");

            migrationBuilder.RenameColumn(
                name: "prize_count",
                table: "channel_giveaways",
                newName: "total_count");

            migrationBuilder.AddColumn<int>(
                name: "claimable_prize_at_interval",
                table: "channel_giveaways",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "interval_minutes",
                table: "channel_giveaways",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
