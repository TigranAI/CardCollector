﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.DataBase;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.InlineQueryHandler.UserToChat
{
    public class ShowStickersInGroup : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var stickersList = User.Stickers
                .Select(item => item.Sticker)
                .Where(item => item.Contains(InlineQuery.Query))
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickers(ChosenInlineResultCommands.chat_send_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }

        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Group or ChatType.Supergroup or ChatType.Channel;
        }

        public ShowStickersInGroup(User user, BotDatabaseContext context, InlineQuery inlineQuery) : base(user, context,
            inlineQuery)
        {
        }
    }
}