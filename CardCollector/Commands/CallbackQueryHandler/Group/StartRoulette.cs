using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
                await roulette.Start();
        }

        public StartRoulette(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}