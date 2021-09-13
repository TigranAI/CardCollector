using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Commands.InlineQuery
{
    public class EmptyInlineQuery : InlineQuery
    {
        protected override string Command => "";
        
        public override async Task<Telegram.Bot.Types.Message> Execute()
        {
            var results = new List<InlineQueryResult>
            {
                new InlineQueryResultCachedSticker("sticker=", "CAACAgIAAxkBAAIWs2DuY4vB50ARmyRwsgABs_7o5weDaAAC-g4AAmq4cUtH6M1FoN4bxSAE")
            };
            await MessageController.AnswerInlineQuery(InlineQueryId, results);
            return new();
        }

        protected override bool IsMatches(string command)
        {
            return command == Command;
        }

        public EmptyInlineQuery(UserEntity user, Update update, string inlineQueryId)
            : base(user, update, inlineQueryId) { }
        public EmptyInlineQuery() { }
    }
}