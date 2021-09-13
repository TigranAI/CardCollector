using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Commands.InlineQuery
{
    public class ShowStickersInGroup : InlineQuery
    {
        protected override string Command => "";
        
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var filter = Update.InlineQuery!.Id;
            var stickersList = GetStickersList(filter);
            
            await MessageController.AnswerInlineQuery(InlineQueryId, stickersList, "название");
            return new();
        }

        private IEnumerable<InlineQueryResult> GetStickersList(string filter)
        {
            var results = new List<InlineQueryResult>
            {
                new InlineQueryResultCachedSticker("send_sticker=", "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE")
            };
            return results;
        }

        protected override bool IsMatches(string command)
        {
            return command.Contains(ChatType.Group.ToString()) ||
                   command.Contains(ChatType.Supergroup.ToString()) ||
                   command.Contains(ChatType.Private.ToString());
        }

        public ShowStickersInGroup(UserEntity user, Update update, string inlineQueryId)
            : base(user, update, inlineQueryId)
        {
            
        }
        public ShowStickersInGroup() { }
    }
}