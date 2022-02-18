using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CardCollector.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            /*migrationBuilder.CreateTable(
                name: "count_logs",
                columns: table => new
                {
                    date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    people_collected_income_one_to_three_times = table.Column<int>(type: "int", nullable: false),
                    people_collected_income_more_times = table.Column<int>(type: "int", nullable: false),
                    people_completed_daily_task = table.Column<int>(type: "int", nullable: false),
                    people_sends_sticker_one_or_more_times = table.Column<int>(type: "int", nullable: false),
                    people_donated = table.Column<int>(type: "int", nullable: false),
                    people_puts_sticker_to_auction = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_count_logs", x => x.date);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    level_value = table.Column<int>(type: "int", nullable: false),
                    level_exp_goal = table.Column<long>(type: "bigint", nullable: false),
                    level_reward = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_levels", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "packs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    author = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price_coins = table.Column<int>(type: "int", nullable: false),
                    price_gems = table.Column<int>(type: "int", nullable: false),
                    preview_file_id = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_preview_animated = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    opened_count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_packs", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    username = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_blocked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    privilege_level = table.Column<int>(type: "int", nullable: false),
                    first_reward = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    level_level = table.Column<int>(type: "int", nullable: false),
                    level_current_exp = table.Column<long>(type: "bigint", nullable: false),
                    level_total_exp = table.Column<long>(type: "bigint", nullable: false),
                    cash_coins = table.Column<int>(type: "int", nullable: false),
                    cash_gems = table.Column<int>(type: "int", nullable: false),
                    cash_max_capacity = table.Column<int>(type: "int", nullable: false),
                    cash_last_payout = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    settings_settings = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    messages_menu_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_collect_income_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_top_users_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_daily_task_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_daily_task_alert_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_daily_task_progress_message_id = table.Column<int>(type: "int", nullable: false),
                    messages_chat_messages = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    messages_chat_stickers = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "special_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pack_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_infinite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    price_coins = table.Column<int>(type: "int", nullable: false),
                    price_gems = table.Column<int>(type: "int", nullable: false),
                    discount = table.Column<int>(type: "int", nullable: false),
                    time_limited = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    time_limit = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    additional_prize = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    preview_file_id = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_preview_animated = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_special_orders_packs_pack_id",
                        column: x => x.pack_id,
                        principalTable: "packs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stickers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    author = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    income = table.Column<int>(type: "int", nullable: false),
                    income_time = table.Column<int>(type: "int", nullable: false),
                    tier = table.Column<int>(type: "int", nullable: false),
                    effect = table.Column<int>(type: "int", nullable: false),
                    emoji = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pack_id = table.Column<int>(type: "int", nullable: false),
                    file_id = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_animated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    for_sale_file_id = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_for_sale_animated = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stickers", x => x.id);
                    table.ForeignKey(
                        name: "fk_stickers_packs_pack_id",
                        column: x => x.pack_id,
                        principalTable: "packs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "daily_tasks",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    task_id = table.Column<int>(type: "int", nullable: false),
                    progress = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_daily_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_daily_tasks_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    total_amount = table.Column<int>(type: "int", nullable: false),
                    invoice_payload = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telegram_payment_charge_id = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    provider_payment_charge_id = table.Column<string>(type: "varchar(127)", maxLength: 127, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_activities",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    action = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_activities_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_send_stickers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    chat_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_send_stickers", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_send_stickers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users_packs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    pack_id = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_packs", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_packs_packs_pack_id",
                        column: x => x.pack_id,
                        principalTable: "packs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_packs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "special_order_users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    order_id = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_special_order_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_special_order_users_special_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "special_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_special_order_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "auctions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    trader_id = table.Column<long>(type: "bigint", nullable: false),
                    sticker_id = table.Column<long>(type: "bigint", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auctions", x => x.id);
                    table.ForeignKey(
                        name: "fk_auctions_stickers_sticker_id",
                        column: x => x.sticker_id,
                        principalTable: "stickers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_auctions_users_trader_id",
                        column: x => x.trader_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_stickers",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    sticker_id = table.Column<long>(type: "bigint", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    payout = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    give_prize_date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_stickers", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_stickers_stickers_sticker_id",
                        column: x => x.sticker_id,
                        principalTable: "stickers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_stickers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_auctions_sticker_id",
                table: "auctions",
                column: "sticker_id");

            migrationBuilder.CreateIndex(
                name: "ix_auctions_trader_id",
                table: "auctions",
                column: "trader_id");

            migrationBuilder.CreateIndex(
                name: "ix_daily_tasks_user_id",
                table: "daily_tasks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_user_id",
                table: "payments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_order_users_order_id",
                table: "special_order_users",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_order_users_user_id",
                table: "special_order_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_special_orders_pack_id",
                table: "special_orders",
                column: "pack_id");

            migrationBuilder.CreateIndex(
                name: "ix_stickers_pack_id",
                table: "stickers",
                column: "pack_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_activities_user_id",
                table: "user_activities",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_send_stickers_user_id",
                table: "user_send_stickers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_stickers_sticker_id",
                table: "user_stickers",
                column: "sticker_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_stickers_user_id",
                table: "user_stickers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_packs_pack_id",
                table: "users_packs",
                column: "pack_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_packs_user_id",
                table: "users_packs",
                column: "user_id");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auctions");

            migrationBuilder.DropTable(
                name: "count_logs");

            migrationBuilder.DropTable(
                name: "daily_tasks");

            migrationBuilder.DropTable(
                name: "levels");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "special_order_users");

            migrationBuilder.DropTable(
                name: "user_activities");

            migrationBuilder.DropTable(
                name: "user_send_stickers");

            migrationBuilder.DropTable(
                name: "user_stickers");

            migrationBuilder.DropTable(
                name: "users_packs");

            migrationBuilder.DropTable(
                name: "special_orders");

            migrationBuilder.DropTable(
                name: "stickers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "packs");
        }
    }
}
