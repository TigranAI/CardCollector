using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    public class StartRoulette : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.start_roulette;

        protected override async Task Execute()
        {
            Logs.LogOut("here");
            var rouletteId = long.Parse(CallbackQuery.Data!.Split("=")[1]);
            Logs.LogOut("here");
            var roulette = await Context.ChatRoulette.FindById(rouletteId);
            Logs.LogOut("here");
            if (roulette == null) return;
            Logs.LogOut("here");
            if (roulette.Creator.Id != User.Id)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_not_creator);
            else
                await roulette.Start(Context);
        }
    }
}