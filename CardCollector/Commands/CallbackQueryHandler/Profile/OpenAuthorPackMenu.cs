﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class OpenAuthorPackMenu : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.open_author_pack_menu;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packs = User.Packs.Where(item => item.Count > 0 && item.Pack.Id != 1).ToList();
            if (packs.Count == 0)
            {
                if (User.Session.PreviousCommandType == typeof(OpenPack))
                {
                    User.Session.PopLast();
                    await new Back(User, Context, CallbackQuery).PrepareAndExecute();
                }
                else await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.packs_count_zero, true);
            }
            else
            {
                var offset = int.Parse(CallbackQuery.Data!.Split('=')[1]);
                var totalCount = packs.Count;
                packs = packs.Skip(offset).Take(10).ToList();
                if (packs.Count == 0)
                    await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
                else
                    await User.Messages.EditMessage(User, Messages.choose_author,
                        Keyboard.GetUserPacksKeyboard(packs, offset, totalCount));
            }
        }

        public OpenAuthorPackMenu(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}