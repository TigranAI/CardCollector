using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class SplitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_cash",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    coins = table.Column<int>(type: "int", nullable: false),
                    gems = table.Column<int>(type: "int", nullable: false),
                    max_capacity = table.Column<int>(type: "int", nullable: false),
                    last_payout = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_cash", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_cash_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_level",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    current_exp = table.Column<long>(type: "bigint", nullable: false),
                    total_exp = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_level", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_level_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_messages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    menu_message_id = table.Column<int>(type: "int", nullable: false),
                    collect_income_message_id = table.Column<int>(type: "int", nullable: false),
                    top_users_message_id = table.Column<int>(type: "int", nullable: false),
                    daily_task_message_id = table.Column<int>(type: "int", nullable: false),
                    daily_task_alert_message_id = table.Column<int>(type: "int", nullable: false),
                    daily_task_progress_message_id = table.Column<int>(type: "int", nullable: false),
                    chat_messages = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    chat_stickers = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_messages_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    settings = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_settings", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_settings_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(
                "INSERT INTO user_cash (id, coins, gems, max_capacity, last_payout)" +
                "SELECT id, cash_coins, cash_gems, cash_max_capacity, cash_last_payout FROM users"
                );

            migrationBuilder.Sql(
                "INSERT INTO user_level (id, level, current_exp, total_exp) " +
                "SELECT id, level_level, level_current_exp, level_total_exp FROM users"
                );

            migrationBuilder.Sql(
                "INSERT INTO user_messages (id, menu_message_id, collect_income_message_id, top_users_message_id, daily_task_message_id, daily_task_alert_message_id, daily_task_progress_message_id, chat_messages, chat_stickers) " +
                "SELECT id, messages_menu_message_id, messages_collect_income_message_id, messages_top_users_message_id, messages_daily_task_message_id, messages_daily_task_alert_message_id, messages_daily_task_progress_message_id, messages_chat_messages, messages_chat_stickers FROM users"
                );

            migrationBuilder.Sql(
                "INSERT INTO user_settings (id, settings)" +
                "SELECT id, settings_settings FROM users"
                );
            
            migrationBuilder.DropColumn(
                name: "cash_coins",
                table: "users");

            migrationBuilder.DropColumn(
                name: "cash_gems",
                table: "users");

            migrationBuilder.DropColumn(
                name: "cash_last_payout",
                table: "users");

            migrationBuilder.DropColumn(
                name: "cash_max_capacity",
                table: "users");

            migrationBuilder.DropColumn(
                name: "level_current_exp",
                table: "users");

            migrationBuilder.DropColumn(
                name: "level_level",
                table: "users");

            migrationBuilder.DropColumn(
                name: "level_total_exp",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_chat_messages",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_chat_stickers",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_collect_income_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_daily_task_alert_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_daily_task_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_daily_task_progress_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_menu_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "messages_top_users_message_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "settings_settings",
                table: "users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cash_coins",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "cash_gems",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "cash_last_payout",
                table: "users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "cash_max_capacity",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "level_current_exp",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "level_level",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "level_total_exp",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "messages_chat_messages",
                table: "users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "messages_chat_stickers",
                table: "users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "messages_collect_income_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messages_daily_task_alert_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messages_daily_task_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messages_daily_task_progress_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messages_menu_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "messages_top_users_message_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "settings_settings",
                table: "users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.Sql(
                "UPDATE users u, user_cash uc " +
                "SET u.cash_coins = uc.coins, u.cash_gems = uc.gems, u.cash_max_capacity = uc.max_capacity, u.cash_last_payout = uc.last_payout " +
                "WHERE u.id == uc.id"
            );

            migrationBuilder.Sql(
                "UPDATE users u, user_level ul " +
                "SET u.level_level = ul.level, u.level_current_exp = ul.current_exp, u.level_total_exp = ul.total_exp " +
                "WHERE u.id == ul.id"
            );

            migrationBuilder.Sql(
                "UPDATE users u, user_messages um " +
                "SET u.messages_menu_message_id = um.menu_message_id, u.messages_collect_income_message_id = um.collect_income_message_id, u.messages_top_users_message_id = um.top_users_message_id, u.messages_daily_task_message_id = um.daily_task_message_id, u.messages_daily_task_alert_message_id = um.daily_task_alert_message_id, u.messages_daily_task_progress_message_id = um.daily_task_progress_message_id, u.messages_chat_messages = um.chat_messages, u.messages_chat_stickers = um.chat_stickers " +
                "WHERE u.id == um.id"
            );

            migrationBuilder.Sql(
                "UPDATE users u, user_settings us " +
                "SET u.settings_settings = us.settings " +
                "WHERE u.id == us.id"
            );
            
            migrationBuilder.DropTable(
                name: "user_cash");

            migrationBuilder.DropTable(
                name: "user_level");

            migrationBuilder.DropTable(
                name: "user_messages");

            migrationBuilder.DropTable(
                name: "user_settings");
        }
    }
}
