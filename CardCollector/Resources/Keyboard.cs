using System;
using System.Collections.Generic;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.MessageHandler;
using CardCollector.DataBase.Entity;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.ReplyMarkups;

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
                        $"{Text.daily_tasks} {(settings[UserSettingsEnum.DailyTasks] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.DailyTasks}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.sticker_effects} {(settings[UserSettingsEnum.StickerEffects] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.StickerEffects}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.exp_gain} {(settings[UserSettingsEnum.ExpGain] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.ExpGain}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.daily_task_progress} {(settings[UserSettingsEnum.DailyTaskProgress] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.DailyTaskProgress}")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.piggy_bank_capacity} {(settings[UserSettingsEnum.PiggyBankCapacity] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.PiggyBankCapacity}"),
                    InlineKeyboardButton.WithCallbackData(
                        $"{Text.daily_exp_top} {(settings[UserSettingsEnum.DailyExpTop] ? Text.alert_on : Text.alert_off)}",
                        $"{CallbackQueryCommands.alerts}={(int) UserSettingsEnum.DailyExpTop}")
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
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.sort}={SortingTypes.None}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierIncrease,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.sort}={SortingTypes.ByTierIncrease}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierDecrease,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.sort}={SortingTypes.ByTierDecrease}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByAuthor,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.sort}={SortingTypes.ByAuthor}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(SortingTypes.ByTitle,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.sort}={SortingTypes.ByTitle}")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню выбора тира */
        public static readonly InlineKeyboardMarkup TierOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.all,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.tier}=-1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("1",
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.tier}=1")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("2",
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.tier}=2")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("3",
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.tier}=3")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("4",
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.tier}=4")
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню ввода эмоджи */
        public static readonly InlineKeyboardMarkup EmojiOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.all,
                    $"{CallbackQueryCommands.set}={CallbackQueryCommands.emoji}=")
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
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsFrom}=0"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 100",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsTo}=100"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 100",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsFrom}=100"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 500",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsTo}=500"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 500",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsFrom}=500"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 1000",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsTo}=1000"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 1000",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsFrom}=1000"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} ∞",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceCoinsTo}"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup GemsPriceOptions = new(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 0",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsFrom}=0"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 10",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsTo}=10"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 10",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsFrom}=10"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 50",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsTo}=50"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 50",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsFrom}=50"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 100",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsTo}=100"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 100",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsFrom}=100"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} ∞",
                    $"{CallbackQueryCommands.set}={(int) FiltersModule.FilterKeys.PriceGemsTo}"),
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
                        $"{CallbackQueryCommands.set}={CallbackQueryCommands.authors_menu}=")
                }
            };
            foreach (var (author, i) in list.WithIndex())
            {
                if (i % 2 == 0)
                    keyboardList.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(author.Author,
                            $"{CallbackQueryCommands.set}={CallbackQueryCommands.authors_menu}={author.Author}")
                    });
                else
                    keyboardList[keyboardList.Count - 1] = new[]
                    {
                        keyboardList[keyboardList.Count - 1][0],
                        InlineKeyboardButton.WithCallbackData(author.Author,
                            $"{CallbackQueryCommands.set}={CallbackQueryCommands.authors_menu}={author.Author}")
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
                if (user.HasAuctionDiscount()) price = (int) (price * 0.05);
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
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back)},
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.select_another)},
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
                        $"{order.GetResultPriceCoins()}{Text.coin}", $"{CallbackQueryCommands.buy_special_order}=coins")
                });
            if (order.GetResultPriceGems() >= 0)
                if (keyboard.Count > 0)
                    keyboard[0] = new[]
                    {
                        keyboard[0][0],
                        InlineKeyboardButton.WithCallbackData($"{order.GetResultPriceGems()}{Text.gem}",
                            $"{CallbackQueryCommands.buy_special_order}=gems")
                    };
                else
                    keyboard.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{order.GetResultPriceGems()}{Text.gem}",
                            $"{CallbackQueryCommands.buy_special_order}=gems")
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
                        $"{pack.PriceCoins}{Text.coin}", $"{CallbackQueryCommands.buy_pack}=coins")
                });
            if (pack.PriceGems >= 0)
                if (keyboard.Count > 0)
                    keyboard[0] = new[]
                    {
                        keyboard[0][0],
                        InlineKeyboardButton.WithCallbackData($"{pack.PriceGems}{Text.gem}",
                            $"{CallbackQueryCommands.buy_pack}=gems")
                    };
                else
                    keyboard.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($"{pack.PriceGems}{Text.gem}",
                            $"{CallbackQueryCommands.buy_pack}=gems")
                    });
            keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.info, CallbackQueryCommands.show_order_info)});
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
    }
}