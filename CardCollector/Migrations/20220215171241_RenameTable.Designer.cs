﻿// <auto-generated />
using System;
using CardCollector.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CardCollector.Migrations
{
    [DbContext(typeof(BotDatabaseContext))]
    [Migration("20220215171241_RenameTable")]
    partial class RenameTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CardCollector.DataBase.Entity.Auction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<int>("Price")
                        .HasColumnType("int")
                        .HasColumnName("price");

                    b.Property<long>("StickerId")
                        .HasColumnType("bigint")
                        .HasColumnName("sticker_id");

                    b.Property<long>("TraderId")
                        .HasColumnType("bigint")
                        .HasColumnName("trader_id");

                    b.HasKey("Id")
                        .HasName("pk_auctions");

                    b.HasIndex("StickerId")
                        .HasDatabaseName("ix_auctions_sticker_id");

                    b.HasIndex("TraderId")
                        .HasDatabaseName("ix_auctions_trader_id");

                    b.ToTable("auctions", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.CountLogs", b =>
                {
                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date");

                    b.Property<int>("PeopleCollectedIncomeMoreTimes")
                        .HasColumnType("int")
                        .HasColumnName("people_collected_income_more_times");

                    b.Property<int>("PeopleCollectedIncomeOneToThreeTimes")
                        .HasColumnType("int")
                        .HasColumnName("people_collected_income_one_to_three_times");

                    b.Property<int>("PeopleCompletedDailyTask")
                        .HasColumnType("int")
                        .HasColumnName("people_completed_daily_task");

                    b.Property<int>("PeopleDonated")
                        .HasColumnType("int")
                        .HasColumnName("people_donated");

                    b.Property<int>("PeoplePutsStickerToAuction")
                        .HasColumnType("int")
                        .HasColumnName("people_puts_sticker_to_auction");

                    b.Property<int>("PeopleSendsStickerOneOrMoreTimes")
                        .HasColumnType("int")
                        .HasColumnName("people_sends_sticker_one_or_more_times");

                    b.HasKey("Date")
                        .HasName("pk_count_logs");

                    b.ToTable("count_logs", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.DailyTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("Progress")
                        .HasColumnType("int")
                        .HasColumnName("progress");

                    b.Property<int>("TaskId")
                        .HasColumnType("int")
                        .HasColumnName("task_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_daily_tasks");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_daily_tasks_user_id");

                    b.ToTable("daily_tasks", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<long>("LevelExpGoal")
                        .HasColumnType("bigint")
                        .HasColumnName("level_exp_goal");

                    b.Property<string>("LevelReward")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("level_reward");

                    b.Property<int>("LevelValue")
                        .HasColumnType("int")
                        .HasColumnName("level_value");

                    b.HasKey("Id")
                        .HasName("pk_levels");

                    b.ToTable("levels", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Pack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("author");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("description");

                    b.Property<bool?>("IsPreviewAnimated")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_preview_animated");

                    b.Property<long>("OpenedCount")
                        .HasColumnType("bigint")
                        .HasColumnName("opened_count");

                    b.Property<string>("PreviewFileId")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("preview_file_id");

                    b.Property<int>("PriceCoins")
                        .HasColumnType("int")
                        .HasColumnName("price_coins");

                    b.Property<int>("PriceGems")
                        .HasColumnType("int")
                        .HasColumnName("price_gems");

                    b.HasKey("Id")
                        .HasName("pk_packs");

                    b.ToTable("packs", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("InvoicePayload")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)")
                        .HasColumnName("invoice_payload");

                    b.Property<string>("ProviderPaymentChargeId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("provider_payment_charge_id");

                    b.Property<string>("TelegramPaymentChargeId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("telegram_payment_charge_id");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int")
                        .HasColumnName("total_amount");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_payments");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_payments_user_id");

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.SpecialOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("AdditionalPrize")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("additional_prize");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<string>("Description")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("description");

                    b.Property<int>("Discount")
                        .HasColumnType("int")
                        .HasColumnName("discount");

                    b.Property<bool>("IsInfinite")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_infinite");

                    b.Property<bool?>("IsPreviewAnimated")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_preview_animated");

                    b.Property<int>("PackId")
                        .HasColumnType("int")
                        .HasColumnName("pack_id");

                    b.Property<string>("PreviewFileId")
                        .HasColumnType("longtext")
                        .HasColumnName("preview_file_id");

                    b.Property<int>("PriceCoins")
                        .HasColumnType("int")
                        .HasColumnName("price_coins");

                    b.Property<int>("PriceGems")
                        .HasColumnType("int")
                        .HasColumnName("price_gems");

                    b.Property<DateTime?>("TimeLimit")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("time_limit");

                    b.Property<bool>("TimeLimited")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("time_limited");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_special_orders");

                    b.HasIndex("PackId")
                        .HasDatabaseName("ix_special_orders_pack_id");

                    b.ToTable("special_orders", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.SpecialOrderUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnName("order_id");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("timestamp");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_special_order_users");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_special_order_users_order_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_special_order_users_user_id");

                    b.ToTable("special_order_users", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Sticker", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)")
                        .HasColumnName("author");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)")
                        .HasColumnName("description");

                    b.Property<int>("Effect")
                        .HasColumnType("int")
                        .HasColumnName("effect");

                    b.Property<string>("Emoji")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("emoji");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("file_id");

                    b.Property<string>("ForSaleFileId")
                        .HasMaxLength(127)
                        .HasColumnType("varchar(127)")
                        .HasColumnName("for_sale_file_id");

                    b.Property<int>("Income")
                        .HasColumnType("int")
                        .HasColumnName("income");

                    b.Property<int>("IncomeTime")
                        .HasColumnType("int")
                        .HasColumnName("income_time");

                    b.Property<bool>("IsAnimated")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_animated");

                    b.Property<bool?>("IsForSaleAnimated")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_for_sale_animated");

                    b.Property<int>("PackId")
                        .HasColumnType("int")
                        .HasColumnName("pack_id");

                    b.Property<int>("Tier")
                        .HasColumnType("int")
                        .HasColumnName("tier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_stickers");

                    b.HasIndex("PackId")
                        .HasDatabaseName("ix_stickers_pack_id");

                    b.ToTable("stickers", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    b.Property<bool>("FirstReward")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("first_reward");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_blocked");

                    b.Property<int>("PrivilegeLevel")
                        .HasColumnType("int")
                        .HasColumnName("privilege_level");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserActivity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Action")
                        .HasColumnType("longtext")
                        .HasColumnName("action");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("timestamp");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_activities");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_activities_user_id");

                    b.ToTable("user_activities", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserPacks", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<int>("PackId")
                        .HasColumnType("int")
                        .HasColumnName("pack_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_packs");

                    b.HasIndex("PackId")
                        .HasDatabaseName("ix_user_packs_pack_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_packs_user_id");

                    b.ToTable("user_packs", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserSendStickerToChat", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_send_stickers");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_send_stickers_user_id");

                    b.ToTable("user_send_stickers", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserSticker", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int>("Count")
                        .HasColumnType("int")
                        .HasColumnName("count");

                    b.Property<DateTime>("GivePrizeDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("give_prize_date");

                    b.Property<DateTime>("Payout")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("payout");

                    b.Property<long>("StickerId")
                        .HasColumnType("bigint")
                        .HasColumnName("sticker_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_stickers");

                    b.HasIndex("StickerId")
                        .HasDatabaseName("ix_user_stickers_sticker_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_stickers_user_id");

                    b.ToTable("user_stickers", (string)null);
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Auction", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.Sticker", "Sticker")
                        .WithMany()
                        .HasForeignKey("StickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_auctions_stickers_sticker_id");

                    b.HasOne("CardCollector.DataBase.Entity.User", "Trader")
                        .WithMany()
                        .HasForeignKey("TraderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_auctions_users_trader_id");

                    b.Navigation("Sticker");

                    b.Navigation("Trader");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.DailyTask", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany("DailyTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_daily_tasks_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Payment", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_payments_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.SpecialOrder", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.Pack", "Pack")
                        .WithMany()
                        .HasForeignKey("PackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_special_orders_packs_pack_id");

                    b.Navigation("Pack");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.SpecialOrderUser", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.SpecialOrder", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_special_order_users_special_orders_order_id");

                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany("SpecialOrdersUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_special_order_users_users_user_id");

                    b.Navigation("Order");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Sticker", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.Pack", "Pack")
                        .WithMany("Stickers")
                        .HasForeignKey("PackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_stickers_packs_pack_id");

                    b.Navigation("Pack");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.User", b =>
                {
                    b.OwnsOne("CardCollector.DataBase.Entity.Cash", "Cash", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<int>("Coins")
                                .HasColumnType("int")
                                .HasColumnName("cash_coins");

                            b1.Property<int>("Gems")
                                .HasColumnType("int")
                                .HasColumnName("cash_gems");

                            b1.Property<DateTime>("LastPayout")
                                .HasColumnType("datetime(6)")
                                .HasColumnName("cash_last_payout");

                            b1.Property<int>("MaxCapacity")
                                .HasColumnType("int")
                                .HasColumnName("cash_max_capacity");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.OwnsOne("CardCollector.DataBase.Entity.UserLevel", "Level", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("CurrentExp")
                                .HasColumnType("bigint")
                                .HasColumnName("level_current_exp");

                            b1.Property<int>("Level")
                                .HasColumnType("int")
                                .HasColumnName("level_level");

                            b1.Property<long>("TotalExp")
                                .HasColumnType("bigint")
                                .HasColumnName("level_total_exp");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.OwnsOne("CardCollector.DataBase.Entity.UserMessages", "Messages", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("ChatMessages")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("messages_chat_messages");

                            b1.Property<string>("ChatStickers")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("messages_chat_stickers");

                            b1.Property<int>("CollectIncomeMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_collect_income_message_id");

                            b1.Property<int>("DailyTaskAlertMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_daily_task_alert_message_id");

                            b1.Property<int>("DailyTaskMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_daily_task_message_id");

                            b1.Property<int>("DailyTaskProgressMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_daily_task_progress_message_id");

                            b1.Property<int>("MenuMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_menu_message_id");

                            b1.Property<int>("TopUsersMessageId")
                                .HasColumnType("int")
                                .HasColumnName("messages_top_users_message_id");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.OwnsOne("CardCollector.DataBase.Entity.UserSettings", "Settings", b1 =>
                        {
                            b1.Property<long>("UserId")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<string>("Settings")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("settings_settings");

                            b1.HasKey("UserId");

                            b1.ToTable("users");

                            b1.WithOwner()
                                .HasForeignKey("UserId")
                                .HasConstraintName("fk_users_users_id");
                        });

                    b.Navigation("Cash")
                        .IsRequired();

                    b.Navigation("Level")
                        .IsRequired();

                    b.Navigation("Messages")
                        .IsRequired();

                    b.Navigation("Settings")
                        .IsRequired();
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserActivity", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_activities_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserPacks", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.Pack", "Pack")
                        .WithMany()
                        .HasForeignKey("PackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_packs_packs_pack_id");

                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany("Packs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_packs_users_user_id");

                    b.Navigation("Pack");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserSendStickerToChat", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_send_stickers_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.UserSticker", b =>
                {
                    b.HasOne("CardCollector.DataBase.Entity.Sticker", "Sticker")
                        .WithMany()
                        .HasForeignKey("StickerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_stickers_stickers_sticker_id");

                    b.HasOne("CardCollector.DataBase.Entity.User", "User")
                        .WithMany("Stickers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_stickers_users_user_id");

                    b.Navigation("Sticker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.Pack", b =>
                {
                    b.Navigation("Stickers");
                });

            modelBuilder.Entity("CardCollector.DataBase.Entity.User", b =>
                {
                    b.Navigation("DailyTasks");

                    b.Navigation("Packs");

                    b.Navigation("SpecialOrdersUser");

                    b.Navigation("Stickers");
                });
#pragma warning restore 612, 618
        }
    }
}
