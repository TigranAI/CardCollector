﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class ShowTopBy : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_top_by;
        protected override async Task Execute()
        {
            var topBy = (TopBy) int.Parse(CallbackQuery.Data!.Split('=')[1]);
            switch (topBy)
            {
                case TopBy.Exp:
                    var topByExp = Messages.users_top_exp;
                    var usersExpTop = await Context.Users.FindTopByExp();
                    foreach (var (user, index) in usersExpTop.WithIndex())
                    {
                        topByExp += $"\n{index+1}.{user.Username}: {user.Level.TotalExp} {Text.exp}";
                    }
                    await User.Messages.SendTopUsers(User, topByExp, Keyboard.GetTopButton(TopBy.Tier4Stickers));
                    break;
                case TopBy.Tier4Stickers:
                    var usersTier4Top = await Context.Users.FindTopByTier4Stickers();
                    var topByTier4 = Messages.users_top_tier_4_stickers_count;
                    foreach (var (user, index) in usersTier4Top.WithIndex())
                    {
                        var count = user.Stickers.Where(sticker => sticker.Sticker.Tier == 4).Count();
                        topByTier4 += $"\n{index+1}.{user.Username}: {count} {Text.stickers}";
                    }
                    await User.Messages.SendTopUsers(User, topByTier4, Keyboard.GetTopButton(TopBy.Exp));
                    break;
            }
        }

        public ShowTopBy(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}