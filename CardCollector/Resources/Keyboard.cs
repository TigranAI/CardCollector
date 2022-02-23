using System;
using System.Collections.Generic;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.ReplyMarkups;
using SortingTypes = CardCollector.Resources.Translations.SortingTypes;
using UserSettings = CardCollector.Database.Entity.UserSettings;

namespace CardCollector.Resources
{
    public static class Keyboard
    {
        public static readonly ReplyKeyboardMarkup Menu = new(new[]
        {
            new KeyboardButton[] {MessageCommands.profile, MessageCommands.collection},
            new KeyboardButton[] {MessageCommands.shop, MessageCommands.auction},
        }) {ResizeKeyboard = true};

        public static readonly InlineKeyboardMarkup PackMenu = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_random, $"{CallbackQueryCommands.open_pack}=1")},
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.open_author,
                    $"{CallbackQueryCommands.open_author_pack_menu}=0")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup StopKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.stop_bot, CallbackQueryCommands.stop_bot)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static InlineKeyboardMarkup ShowStickers = new(new[]
        {
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static InlineKeyboardMarkup BackShopKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_packs, CallbackQueryCommands.my_packs)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static InlineKeyboardMarkup BackAndMoreKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static InlineKeyboardMarkup ProviderKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData("Сбербанк", $"{CallbackQueryCommands.buy_gems}=Сбербанк")},
            new[] {InlineKeyboardButton.WithCallbackData("ЮКасса", $"{CallbackQueryCommands.buy_gems}=ЮКасса")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static InlineKeyboardMarkup GiveawayKeyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.random_pack,
                    $"{CallbackQueryCommands.set_giveaway_prize}={(int) PrizeType.RandomPack}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.random_sticker,
                    $"{CallbackQueryCommands.set_giveaway_prize}={(int) PrizeType.RandomSticker}")
            },
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.choose_sticker)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup SelectChannel = new(new[]
        {
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.choose_channel)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup CreateGiveaway = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.confirm, CallbackQueryCommands.confirm_giveaway)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup GiveawayTier = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1", $"{CallbackQueryCommands.select_giveaway_tier}=1"),
                InlineKeyboardButton.WithCallbackData("2", $"{CallbackQueryCommands.select_giveaway_tier}=2"),
                InlineKeyboardButton.WithCallbackData("3", $"{CallbackQueryCommands.select_giveaway_tier}=3"),
                InlineKeyboardButton.WithCallbackData("4", $"{CallbackQueryCommands.select_giveaway_tier}=4"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup RouletteKeyboard(long rouletteId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.place_a_bet,
                        InlineQueryCommands.roulette)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.roulette_rule, CallbackQueryCommands.roulette_rule)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.start_roulette,
                        $"{CallbackQueryCommands.start_roulette}={rouletteId}")
                }
            });
        }

        public static InlineKeyboardMarkup BuyCoinsKeyboard(bool confirmButton = false)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"300{Text.coin}",
                        $"{CallbackQueryCommands.set_exchange_sum}=30"),
                    InlineKeyboardButton.WithCallbackData($"600{Text.coin}",
                        $"{CallbackQueryCommands.set_exchange_sum}=60"),
                    InlineKeyboardButton.WithCallbackData($"900{Text.coin}",
                        $"{CallbackQueryCommands.set_exchange_sum}=90")
                }
            };
            if (confirmButton)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.confirm_exchange,
                        CallbackQueryCommands.confirm_exchange)
                });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static readonly InlineKeyboardMarkup BuyGems = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"100{Text.gem}", $"{CallbackQueryCommands.set_gems_sum}=100"),
                InlineKeyboardButton.WithCallbackData($"300{Text.gem}", $"{CallbackQueryCommands.set_gems_sum}=300"),
                InlineKeyboardButton.WithCallbackData($"700{Text.gem}", $"{CallbackQueryCommands.set_gems_sum}=700")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
        });

        public static readonly InlineKeyboardMarkup EndStickerUpload = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.end_sticker_upload, CallbackQueryCommands.end_sticker_upload)
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup Settings(PrivilegeLevel privilegeLevel)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.alerts, CallbackQueryCommands.alerts)},
            };
            if (privilegeLevel > PrivilegeLevel.Vip)
                keyboard.Add(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.control_panel, CallbackQueryCommands.control_panel)
                    });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup MyPacks = new(new[]
        {
            InlineKeyboardButton.WithCallbackData(Text.open_packs, CallbackQueryCommands.my_packs)
        });

        public static InlineKeyboardMarkup Alerts(UserSettings settings)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.daily_tasks} {(settings[Enums.UserSettings.DailyTasks] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.DailyTasks}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.sticker_effects} {(settings[Enums.UserSettings.StickerEffects] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.StickerEffects}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.exp_gain} {(settings[Enums.UserSettings.ExpGain] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.ExpGain}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.daily_task_progress} {(settings[Enums.UserSettings.DailyTaskProgress] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.DailyTaskProgress}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.piggy_bank_capacity} {(settings[Enums.UserSettings.PiggyBankCapacity] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.PiggyBankCapacity}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.daily_exp_top} {(settings[Enums.UserSettings.DailyExpTop] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) Enums.UserSettings.DailyExpTop}")
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            };
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup BackToFilters(string stickerTitle)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerTitle)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
            });
        }

        public static InlineKeyboardMarkup GetSortingMenu(UserState state)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.author, $"{CallbackQueryCommands.authors_menu}=0")},
                new[] {InlineKeyboardButton.WithCallbackData(Text.tier, CallbackQueryCommands.tier)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.emoji, CallbackQueryCommands.emoji)}
            };
            if (state != UserState.CollectionMenu)
                keyboard.Add(new[]
                    {InlineKeyboardButton.WithCallbackData(Text.price, CallbackQueryCommands.select_price)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.sort, CallbackQueryCommands.sort)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            return new InlineKeyboardMarkup(keyboard);
        }

        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.no,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Sorting}={(int) Enums.SortingTypes.None}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierIncrease,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Sorting}={(int) Enums.SortingTypes.ByTierIncrease}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierDecrease,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Sorting}={(int) Enums.SortingTypes.ByTierDecrease}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByAuthor,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Sorting}={(int) Enums.SortingTypes.ByAuthor}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTitle,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Sorting}={(int) Enums.SortingTypes.ByTitle}")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню выбора тира */
        public static readonly InlineKeyboardMarkup TierOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.all,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Tier}=")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Tier}=1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("2",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Tier}=2")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("3",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Tier}=3")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("4",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Tier}=4")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню ввода эмоджи */
        public static readonly InlineKeyboardMarkup EmojiOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.all,
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.Emoji}=")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура с одной кнопкой отмены */
        public static readonly InlineKeyboardMarkup BackKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });


        /* Клавиатура с одной кнопкой отмены */
        public static readonly InlineKeyboardMarkup StickerInfoKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура для покупок */
        public static InlineKeyboardMarkup BuyGemsKeyboard(int count)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithPayment($"{Text.buy} {count}{Text.gem} {Text.per} ₽{count * 69 / 100}")
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
        }

        /* Клавиатура с отменой и выставлением */
        public static readonly InlineKeyboardMarkup AuctionPutCancelKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.sell_on_auction, CallbackQueryCommands.confirm_selling)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup CoinsPriceOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 0",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsFrom}=0"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 100",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsTo}=100"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 100",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsFrom}=100"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 500",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsTo}=500"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 500",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsFrom}=500"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 1000",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsTo}=1000"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 1000",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsFrom}=1000"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} ∞",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceCoinsTo}"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup GemsPriceOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 0",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsFrom}=0"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 10",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsTo}=10"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 10",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsFrom}=10"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 50",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsTo}=50"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 50",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsFrom}=50"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 100",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsTo}=100"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 100",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsFrom}=100"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} ∞",
                    $"{CallbackQueryCommands.set}={(int) FilterKeys.PriceGemsTo}"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup GetAuthorsKeyboard(List<Pack> list, int offset, int totalCount)
        {
            var keyboardList = new List<InlineKeyboardButton[]>
            {
                /* Добавляем в список кнопку "Все" */
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.all,
                        $"{CallbackQueryCommands.set}={(int) FilterKeys.Author}=")
                }
            };
            foreach (var (author, i) in list.WithIndex())
            {
                if (i % 2 == 0)
                    keyboardList.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(author.Author,
                            $"{CallbackQueryCommands.set}={(int) FilterKeys.Author}={author.Author}")
                    });
                else
                    keyboardList[keyboardList.Count - 1] = new[]
                    {
                        keyboardList[keyboardList.Count - 1][0],
                        InlineKeyboardButton.WithCallbackData(author.Author,
                            $"{CallbackQueryCommands.set}={(int) FilterKeys.Author}={author.Author}")
                    };
            }

            var arrows = new List<InlineKeyboardButton>();
            if (offset > 9)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.previous, $"{CallbackQueryCommands.authors_menu}={offset - 10}"));
            arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back));
            if (totalCount > offset + list.Count)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.next, $"{CallbackQueryCommands.authors_menu}={offset + list.Count}"));
            keyboardList.Add(arrows.ToArray());
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup GetPacksKeyboard(List<Pack> list,
            int offset, int totalCount, string command)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();
            foreach (var (item, i) in list.WithIndex())
            {
                if (i % 2 == 0)
                    keyboardList.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(item.Author, $"{command}={item.Id}")
                    });
                else
                    keyboardList[keyboardList.Count - 1] = new[]
                    {
                        keyboardList[keyboardList.Count - 1][0],
                        InlineKeyboardButton.WithCallbackData(item.Author, $"{command}={item.Id}")
                    };
            }

            var arrows = new List<InlineKeyboardButton>();
            if (offset > 9)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.previous,
                        $"{CallbackQueryCommands.choose_pack}={command}={offset - 10}"));
            arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back));
            if (totalCount > offset + list.Count)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.next,
                        $"{CallbackQueryCommands.choose_pack}={command}={offset + list.Count}"));
            keyboardList.Add(arrows.ToArray());
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup GetUserPacksKeyboard(List<UserPacks> list, int offset,
            int totalCount)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();
            foreach (var (item, i) in list.WithIndex())
            {
                if (i % 2 == 0)
                    keyboardList.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{item.Pack.Author} ({item.Count}{Text.items})",
                            $"{CallbackQueryCommands.open_pack}={item.Pack.Id}")
                    });
                else
                    keyboardList[keyboardList.Count - 1] = new[]
                    {
                        keyboardList[keyboardList.Count - 1][0],
                        InlineKeyboardButton.WithCallbackData($"{item.Pack.Author} ({item.Count}{Text.items})",
                            $"{CallbackQueryCommands.open_pack}={item.Pack.Id}")
                    };
            }

            var arrows = new List<InlineKeyboardButton>();
            if (offset > 9)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.previous, $"{CallbackQueryCommands.open_author_pack_menu}={offset - 10}"));
            arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back));
            if (totalCount > offset + list.Count)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.next,
                        $"{CallbackQueryCommands.open_author_pack_menu}={offset + list.Count}"));
            keyboardList.Add(arrows.ToArray());
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup GetCollectionStickerKeyboard(Sticker sticker, int count)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, sticker.Title)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"{Text.sell_on_auction} ({count})",
                        CallbackQueryCommands.sell_sticker)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{CallbackQueryCommands.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{CallbackQueryCommands.count}={Text.plus}"),
                }
            };
            if (sticker.Tier != 4)
                keyboard.Add(
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{Text.combine} ({count})",
                            CallbackQueryCommands.combine)
                    });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup AuctionStickerKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_traders)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup GetAuctionProductKeyboard(Auction product, User user, int count)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (product.Trader.Id == user.Id)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.return_from_auction,
                        CallbackQueryCommands.return_from_auction)
                });
            else
            {
                var price = product.Price;
                if (user.HasAuctionDiscount()) price = (int) (price * 0.95);
                keyboard.AddRange(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{Text.buy} ({count}) {price}{Text.gem}",
                            CallbackQueryCommands.confirm_buying)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.minus,
                            $"{CallbackQueryCommands.count}={Text.minus}"),
                        InlineKeyboardButton.WithCallbackData(Text.plus, $"{CallbackQueryCommands.count}={Text.plus}"),
                    },
                });
            }

            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup GetStickerKeyboard(Sticker stickerInfo)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerInfo.Title)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
        }

        public static InlineKeyboardMarkup GetConfirmationKeyboard(string command)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.no, CallbackQueryCommands.back),
                    InlineKeyboardButton.WithCallbackData(Text.yes, command)
                }
            });
        }

        public static InlineKeyboardMarkup GetCombineStickerKeyboard(CombineModule module)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData($"{Text.add} ({module.Count})", CallbackQueryCommands.combine)
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{CallbackQueryCommands.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{CallbackQueryCommands.count}={Text.plus}"),
                },
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.select_another)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
        }

        public static InlineKeyboardMarkup GetCombineKeyboard(CombineModule module)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var (sticker, _) in module.CombineList)
            {
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($"{Text.delete} {Text.sticker} {keyboard.Count + 1}",
                        $"{CallbackQueryCommands.delete_combine}={sticker.Id}")
                });
            }

            if (module.CombineCount == Constants.COMBINE_COUNT)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.combine} {module.CombinePrice}{Text.coin}", CallbackQueryCommands.combine_stickers)
                });
            else keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.add_sticker)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        /* Клавиатура, отображаемая вместе с сообщением профиля */
        public static InlineKeyboardMarkup GetProfileKeyboard(int packsCount, int income = 0)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (income > 0)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($"{Text.collect} {income}{Text.coin}",
                        CallbackQueryCommands.collect_income)
                });
            keyboard.AddRange(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.daily_tasks, CallbackQueryCommands.daily_tasks)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.settings, CallbackQueryCommands.settings),
                    InlineKeyboardButton.WithCallbackData($"{Text.my_packs} {(packsCount > 0 ? Text.gift : "")}",
                        CallbackQueryCommands.my_packs)
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopKeyboard(bool haveOffers, PrivilegeLevel privilegeLevel)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.special_offers + (haveOffers ? Text.gift : ""),
                        CallbackQueryCommands.special_offers)
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_pack, CallbackQueryCommands.buy_pack)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_coins, CallbackQueryCommands.buy_coins)},
            };
            /*if (privilegeLevel >= PrivilegeLevel.Programmer)*/
            keyboard.Add(new[]
                {InlineKeyboardButton.WithCallbackData(Text.buy_gems, CallbackQueryCommands.select_provider)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup SpecialOrdersKeyboard(List<SpecialOrder> orders)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var order in orders)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(order.Title,
                        $"{CallbackQueryCommands.select_order}={order.Id}")
                });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopPacksKeyboard = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.buy_random, $"{CallbackQueryCommands.select_shop_pack}=1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.buy_author,
                    $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.select_shop_pack}=0")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.info, CallbackQueryCommands.pack_info)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        public static InlineKeyboardMarkup OrderKeyboard(SpecialOrder order)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (order.GetResultPriceCoins() >= 0)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{order.GetResultPriceCoins()}{Text.coin}", $"{CallbackQueryCommands.buy_shop_item}=coins")
                });
            if (order.GetResultPriceGems() >= 0)
                if (keyboard.Count > 0)
                    keyboard[0] = new[]
                    {
                        keyboard[0][0],
                        InlineKeyboardButton.WithCallbackData($"{order.GetResultPriceGems()}{Text.gem}",
                            $"{CallbackQueryCommands.buy_shop_item}=gems")
                    };
                else
                    keyboard.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{order.GetResultPriceGems()}{Text.gem}",
                            $"{CallbackQueryCommands.buy_shop_item}=gems")
                    });
            keyboard.Add(
                new[] {InlineKeyboardButton.WithCallbackData(Text.info, CallbackQueryCommands.show_order_info)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopPackKeyboard(Pack pack)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (pack.PriceCoins >= 0)
                keyboard.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{pack.PriceCoins}{Text.coin}", $"{CallbackQueryCommands.buy_shop_item}=coins")
                });
            if (pack.PriceGems >= 0)
                if (keyboard.Count > 0)
                    keyboard[0] = new[]
                    {
                        keyboard[0][0],
                        InlineKeyboardButton.WithCallbackData($"{pack.PriceGems}{Text.gem}",
                            $"{CallbackQueryCommands.buy_shop_item}=gems")
                    };
                else
                    keyboard.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{pack.PriceGems}{Text.gem}",
                            $"{CallbackQueryCommands.buy_shop_item}=gems")
                    });
            if (pack.Id != 1)
                keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            keyboard.Add(
                new[] {InlineKeyboardButton.WithCallbackData(Text.info, CallbackQueryCommands.show_order_info)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup LoginKeyboard(string loginLink)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(Text.login, loginLink));
        }

        public static InlineKeyboardMarkup RepeatCommand(string buttonText, string callbackData)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData(buttonText, callbackData)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
        }

        public static InlineKeyboardMarkup BuyShopItem(string callbackData)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_more, callbackData)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.open_packs, CallbackQueryCommands.my_packs)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
            });
        }

        public static InlineKeyboardMarkup ControlPanel(PrivilegeLevel level)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            if (level >= PrivilegeLevel.Programmer)
                keyboard.AddRange(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.logs_menu,
                            $"{CallbackQueryCommands.logs_menu}={DateTime.Today}")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.create_giveaway,
                            $"{CallbackQueryCommands.create_giveaway}")
                    }
                });
            if (level == PrivilegeLevel.Programmer)
                keyboard.AddRange(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.stop_bot, CallbackQueryCommands.stop_bot)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.upload_stickerpack,
                            CallbackQueryCommands.upload_stickerpack)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.show_sample, CallbackQueryCommands.show_sample)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.add_for_sale_sticker,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.select_for_sale_pack}=0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.add_sticker_preview,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.add_sticker_preview}=0")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(Text.edit_sticker,
                            $"{CallbackQueryCommands.choose_pack}={CallbackQueryCommands.edit_sticker}=0")
                    },
                });
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup LogsMenu(DateTime date)
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.arrow_left,
                    $"{CallbackQueryCommands.logs_menu}={date.AddDays(1)}"),
                InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back),
                InlineKeyboardButton.WithCallbackData(Text.arrow_right,
                    $"{CallbackQueryCommands.logs_menu}={date.AddDays(-1)}"),
            });
        }

        public static InlineKeyboardMarkup GetTopButton(TopBy topBy)
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData(
                    TopByTexts.ResourceManager.GetString(((int) topBy).ToString()) ?? string.Empty,
                    $"{CallbackQueryCommands.show_top_by}={(int) topBy}")
            });
        }

        public static InlineKeyboardMarkup ConfirmLogin(string data)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.confirm_login,
                        $"{CallbackQueryCommands.confirm_login}={data}")
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
            });
        }

        public static InlineKeyboardMarkup SkipKeyboard(string name)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.skip, $"{CallbackQueryCommands.skip}={name}")
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)}
            });
        }

        public static InlineKeyboardMarkup GroupClaimPrize(long chatId, string prize, long prizeId)
        {
            return new InlineKeyboardMarkup(new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.claim, 
                    $"{CallbackQueryCommands.claim_group_prize}={chatId}={prize}={prizeId}")
            });
        }
    }
}