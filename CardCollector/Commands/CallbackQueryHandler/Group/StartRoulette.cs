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
            var rouletteId = long.Parse(CallbackQuery.Data!.Split("=")[1]);
            var roulette = await Context.ChatRoulette.FindById(rouletteId);
            if (roulette == null) return;
            if (roulette.Creator.Id != User.Id)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_not_creator);
            else
                await roulette.Start(Context);
        }
    }
}