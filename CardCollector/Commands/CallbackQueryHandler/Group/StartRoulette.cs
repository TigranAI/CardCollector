using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    public class StartRoulette : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.start_roulette;

        protected override async Task Execute()
        {
            var rouletteId = long.Parse(CallbackQuery.Data!.Split("=")[1]);
            var roulette = await Context.ChatRoulette.FindById(rouletteId);
            if (roulette == null) return;
            if (roulette.Creator.Id != User.Id)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_not_creator);
            else
                await roulette.Start(Context);
        }

        public StartRoulette(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}