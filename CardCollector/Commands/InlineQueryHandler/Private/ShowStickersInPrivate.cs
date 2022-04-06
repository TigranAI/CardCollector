﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Controllers;
using CardCollector.Others;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.InlineQueryHandler.Private
{
    [DontAddToCommandStack]
    public class ShowStickersInPrivate : InlineQueryHandler
    {
        protected override async Task Execute()
        {
            var stickersList = User.Stickers
                .Where(item => item.Count > 0)
                .Select(item => item.Sticker)
                .Where(item => item.Contains(InlineQuery.Query))
                .ToList();
            var offset = int.Parse(InlineQuery.Offset == "" ? "0" : InlineQuery.Offset);
            var newOffset = offset + 50 > stickersList.Count() ? "" : (offset + 50).ToString();
            var results = stickersList
                .ToTelegramStickers(ChosenInlineResultCommands.send_private_sticker, offset);
            await MessageController.AnswerInlineQuery(User, InlineQuery.Id, results, newOffset);
        }
        
        public override bool Match()
        {
            return InlineQuery.ChatType is ChatType.Private;
        }
    }
}