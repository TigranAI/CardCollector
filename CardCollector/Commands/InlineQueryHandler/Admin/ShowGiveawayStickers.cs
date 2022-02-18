﻿using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.Admin
{
    public class ShowGiveawayStickers : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var stickers = await Context.Stickers.ToListAsync();
            var stickersList = stickers
                .Where(item => item.Contains(InlineQuery.Query))
                .Skip(offset)
                .Take(50)
                .ToList();
            var newOffset = offset + 50 > stickersList.Count ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickersAsMessage(ChosenInlineResultCommands.set_giveaway_prize, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            if (User.Session.State is not UserState.CreateGiveaway) return false;
            return User.Session.GetModule<AdminModule>().SelectedChannelGiveawayId == null;
        }

        public ShowGiveawayStickers(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user,
            context, inlineQuery)
        {
        }
    }
}