﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Session;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Resources
{
    /* В данном классе содержатся все клавиатуры, используемые в проекте */
    public static class Keyboard
    {

        /* Клавиатура, отображаемая с первым сообщением пользователя */
        public static readonly ReplyKeyboardMarkup Menu = new(new[]
        {
            new KeyboardButton[] {Text.profile, Text.collection},
            new KeyboardButton[] {Text.shop, Text.auction},
        }) {ResizeKeyboard = true};

        public static readonly InlineKeyboardMarkup PackMenu = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_random, $"{Command.open_pack}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_author, $"{Command.open_author_pack_menu}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        public static readonly InlineKeyboardMarkup BuyCoinsKeyboard = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.confirm_exchange, Command.confirm_exchange)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        public static readonly InlineKeyboardMarkup EndStickerUpload = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.end_sticker_upload, Command.end_sticker_upload)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        public static InlineKeyboardMarkup BackToFilters(string stickerTitle)
        {
            return new InlineKeyboardMarkup(new[]
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerTitle)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)}
            });
        }

        public static InlineKeyboardMarkup GetSortingMenu(UserState state)
        {
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithCallbackData(Text.author, $"{Command.authors_menu}=1")},
                new[] {InlineKeyboardButton.WithCallbackData(Text.tier, Command.tier)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.emoji, Command.emoji)}
            };
            if (state != UserState.CollectionMenu) keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.price, Command.select_price)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.sort, Command.sort)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_stickers)});
            return new InlineKeyboardMarkup(keyboard);
        }
        /* Клавиатура меню сортировки */
        public static readonly InlineKeyboardMarkup SortOptions = new(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.no, $"{Command.set}={Command.sort}={SortingTypes.None}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierIncrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierIncrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTierDecrease, $"{Command.set}={Command.sort}={SortingTypes.ByTierDecrease}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByAuthor, $"{Command.set}={Command.sort}={SortingTypes.ByAuthor}")},
            new[] {InlineKeyboardButton.WithCallbackData(SortingTypes.ByTitle, $"{Command.set}={Command.sort}={SortingTypes.ByTitle}")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура меню выбора тира */
        public static readonly InlineKeyboardMarkup TierOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.tier}=-1")},
            new[] {InlineKeyboardButton.WithCallbackData("1", $"{Command.set}={Command.tier}=1")},
            new[] {InlineKeyboardButton.WithCallbackData("2", $"{Command.set}={Command.tier}=2")},
            new[] {InlineKeyboardButton.WithCallbackData("3", $"{Command.set}={Command.tier}=3")},
            new[] {InlineKeyboardButton.WithCallbackData("4", $"{Command.set}={Command.tier}=4")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура меню ввода эмоджи */
        public static readonly InlineKeyboardMarkup EmojiOptions = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.emoji}=")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура с одной кнопкой отмены */
        public static readonly InlineKeyboardMarkup BackKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура для покупок */
        public static readonly InlineKeyboardMarkup BuyGemsKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithPayment(Text.buy_gems_button)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура с отменой и выставлением */
        public static readonly InlineKeyboardMarkup AuctionPutCancelKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.sell_on_auction, Command.confirm_selling)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup CoinsPriceOptions = new (new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 0", $"{Command.set}={Command.price_coins_from}=0"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 100", $"{Command.set}={Command.price_coins_to}=100"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 100", $"{Command.set}={Command.price_coins_from}=100"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 500", $"{Command.set}={Command.price_coins_to}=500"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 500", $"{Command.set}={Command.price_coins_from}=500"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} 1000", $"{Command.set}={Command.price_coins_to}=1000"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💰 {Text.from} 1000", $"{Command.set}={Command.price_coins_from}=1000"),
                InlineKeyboardButton.WithCallbackData($"💰 {Text.to} ∞", $"{Command.set}={Command.price_coins_to}=0"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        /* Клавиатура меню выбора цен */
        public static readonly InlineKeyboardMarkup GemsPriceOptions = new (new[]
        {
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 0", $"{Command.set}={Command.price_gems_from}=0"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 10", $"{Command.set}={Command.price_gems_to}=10"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 10", $"{Command.set}={Command.price_gems_from}=10"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 50", $"{Command.set}={Command.price_gems_to}=50"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 50", $"{Command.set}={Command.price_gems_from}=50"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} 100", $"{Command.set}={Command.price_gems_to}=100"),
            },
            new[] {
                InlineKeyboardButton.WithCallbackData($"💎 {Text.from} 100", $"{Command.set}={Command.price_gems_from}=100"),
                InlineKeyboardButton.WithCallbackData($"💎 {Text.to} ∞", $"{Command.set}={Command.price_gems_to}=0"),
            },
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        public static InlineKeyboardMarkup GetAuthorsKeyboard(List<string> list, InlineKeyboardButton[] pagePanel)
        {
            var keyboardList = new List<InlineKeyboardButton[]> {
                /* Добавляем в список кнопку "Все" */
                new[] {InlineKeyboardButton.WithCallbackData(Text.all, $"{Command.set}={Command.authors_menu}=")}
            };
            foreach (var (author, i) in list.WithIndex())
            {
                if (i % 2 == 0) keyboardList.Add(new [] {
                    InlineKeyboardButton.WithCallbackData(author, $"{Command.set}={Command.authors_menu}={author}")
                });
                else keyboardList[keyboardList.Count - 1] = new [] {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData(author, $"{Command.set}={Command.authors_menu}={author}")
                };
            }
            keyboardList.Add(pagePanel);
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardButton[] GetPagePanel(int page, int totalCount, string callback)
        {
            var arrows = new List<InlineKeyboardButton>();
            if (page > 1) arrows.Add(InlineKeyboardButton
                .WithCallbackData(Text.previous, $"{callback}={page - 1}"));
            arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, Command.back));
            if (totalCount > page * 10) arrows.Add(InlineKeyboardButton
                .WithCallbackData(Text.next, $"{callback}={page + 1}"));
            return arrows.ToArray();
        }

        public static InlineKeyboardMarkup GetShopPacksKeyboard(List<PackEntity> infoList, InlineKeyboardButton[] pagePanel)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();
            foreach (var (item, i) in infoList.WithIndex())
            {
                if (i % 2 == 0) keyboardList.Add(new [] {
                    InlineKeyboardButton.WithCallbackData(item.Author, $"{Command.select_shop_pack}={item.Id}")
                });
                else keyboardList[keyboardList.Count - 1] = new [] {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData(item.Author, $"{Command.select_shop_pack}={item.Id}")
                };
            }
            keyboardList.Add(pagePanel);
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static async Task<InlineKeyboardMarkup> GetUserPacksKeyboard(List<UserPacks> infoList, InlineKeyboardButton[] pagePanel)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();
            foreach (var (item, i) in infoList.WithIndex())
            {
                var author = await PacksDao.GetById(item.PackId);
                if (i % 2 == 0) keyboardList.Add(new [] {
                    InlineKeyboardButton.WithCallbackData($"{author.Author} ({item.Count}{Text.items})", 
                        $"{Command.open_pack}={item.PackId}")
                });
                else keyboardList[keyboardList.Count - 1] = new [] {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData($"{author.Author} ({item.Count}{Text.items})",
                        $"{Command.open_pack}={item.PackId}")
                };
            }
            keyboardList.Add(pagePanel);
            return new InlineKeyboardMarkup(keyboardList);
        }

        public static InlineKeyboardMarkup GetCollectionStickerKeyboard(CollectionModule module)
        {
            var sticker = module.SelectedSticker;
            var count = module.Count;
            var keyboard = new List<InlineKeyboardButton[]>
            {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, sticker.Title)},
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.sell_on_auction} ({count})", Command.sell_on_auction)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                }
            };
            if (sticker.Tier != 4) keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData($"{Text.combine} ({count})", Command.combine)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup GetAuctionStickerKeyboard()
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.show_traders)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
            });
        }

        public static InlineKeyboardMarkup GetAuctionProductKeyboard(AuctionModule module, double discount)
        {
            var price = (int)(module.Price * module.Count * discount);
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.buy} ({module.Count}) {price}{Text.gem}", Command.confirm_buying)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
            });
        }

        public static InlineKeyboardMarkup GetStickerKeyboard(StickerEntity stickerInfo)
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithSwitchInlineQuery(Text.send_sticker, stickerInfo.Title)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
            });
        }

        public static InlineKeyboardMarkup GetConfirmationKeyboard(string command)
        {
            return new InlineKeyboardMarkup(new[] {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.no, Command.back),
                    InlineKeyboardButton.WithCallbackData(Text.yes, command)
                }
            });
        }

        public static InlineKeyboardMarkup GetStickerKeyboard(UserSession session, double discount = 1.0)
        {
            return session.State switch
            {
                UserState.ProductMenu => GetAuctionProductKeyboard(session.GetModule<AuctionModule>(), discount),
                UserState.AuctionMenu => GetAuctionStickerKeyboard(),
                UserState.CombineMenu => GetCombineStickerKeyboard(session.GetModule<CombineModule>()),
                UserState.CollectionMenu => GetCollectionStickerKeyboard(session.GetModule<CollectionModule>()),
                _ => GetStickerKeyboard(session.GetModule<DefaultModule>().SelectedSticker)
            };
        }

        public static InlineKeyboardMarkup GetCombineStickerKeyboard(CombineModule module)
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.add} ({module.Count})", Command.combine)},
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(Text.minus, $"{Command.count}={Text.minus}"),
                    InlineKeyboardButton.WithCallbackData(Text.plus, $"{Command.count}={Text.plus}"),
                },
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
                new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.select_another)},
            });
        }

        public static InlineKeyboardMarkup GetCombineKeyboard(CombineModule module)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var (sticker, _) in module.CombineList)
            {
                keyboard.Add(new []{InlineKeyboardButton.WithCallbackData($"{Text.delete} {Text.sticker} {keyboard.Count + 1}",
                    $"{Command.delete_combine}={sticker.Md5Hash}")});
            }
            if (module.GetCombineCount() == Constants.COMBINE_COUNT)
                keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(
                    $"{Text.combine} {module.CalculateCombinePrice()}{Text.coin}", Command.combine_stickers)});
            else keyboard.Add(new[] {InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.add_sticker)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        /* Клавиатура, отображаемая вместе с сообщением профиля */
        public static InlineKeyboardMarkup GetProfileKeyboard(int income, int privilegeLevel)
        {
            var keyboard = new List<InlineKeyboardButton[]> {
                new[] {InlineKeyboardButton.WithCallbackData($"{Text.collect} {income}{Text.coin}", Command.collect_income)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.daily_tasks, Command.daily_tasks)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.my_packs, Command.my_packs)},
            };
            if (privilegeLevel > 2) keyboard.Add(
                new[] {InlineKeyboardButton.WithCallbackData(Text.control_panel, Command.control_panel)});
            keyboard.Add(new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopKeyboard(bool haveOffers)
        {
            return new InlineKeyboardMarkup(new[] {
                new[] {InlineKeyboardButton.WithCallbackData(Text.special_offers + (haveOffers ? Text.gift : ""), Command.special_offers)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_pack, Command.buy_pack)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_coins, Command.buy_coins)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.buy_gems, Command.buy_gems)},
                new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
            });
        }

        public static InlineKeyboardMarkup SpecialOffersKeyboard(IEnumerable<ShopEntity> specialOffers)
        {
            var keyboard = new List<InlineKeyboardButton[]>();
            foreach (var offer in specialOffers)
                keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(offer.Title,
                        $"{Command.select_offer}={offer.Id}")
                });
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup ShopPacksKeyboard = new (new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.buy_random, $"{Command.select_shop_pack}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.buy_author, $"{Command.buy_author_pack_menu}=1")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.info, Command.pack_info)},
            new[] {InlineKeyboardButton.WithCallbackData(Text.back, Command.back)},
        });

        public static InlineKeyboardMarkup OfferKeyboard(ShopModule module)
        {
            var resultPriceCoins = module.SelectedPosition?.ResultPriceCoins 
                                   ?? module.SelectedPack?.PriceCoins * module.Count ?? -1; 
            var resultPriceGems = module.SelectedPosition?.ResultPriceGems 
                                  ?? module.SelectedPack?.PriceGems * module.Count ?? -1; 
            var keyboard = new List<InlineKeyboardButton[]>();
            if (resultPriceCoins >= 0)
                keyboard.Add(new [] {InlineKeyboardButton.WithCallbackData(
                    $"{resultPriceCoins}{Text.coin}", $"{Command.buy_shop_item}=coins")
                });
            if (resultPriceGems >= 0)
                if (keyboard.Count > 0) keyboard[0] = new [] {
                    keyboard[0][0], 
                    InlineKeyboardButton.WithCallbackData($"{resultPriceGems}{Text.gem}",
                        $"{Command.buy_shop_item}=gems")
                };
                else keyboard.Add(new [] {InlineKeyboardButton.WithCallbackData($"{resultPriceGems}{Text.gem}",
                    $"{Command.buy_shop_item}=gems")
                });
            keyboard.Add(new [] {InlineKeyboardButton.WithCallbackData(Text.info, Command.show_offer_info)});
            keyboard.Add(new []{InlineKeyboardButton.WithCallbackData(Text.back, Command.back)});
            return new InlineKeyboardMarkup(keyboard);
        }

        public static InlineKeyboardMarkup LoginKeyboard(string loginLink)
        {
            return new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(Text.login, loginLink));
        }
    }
}