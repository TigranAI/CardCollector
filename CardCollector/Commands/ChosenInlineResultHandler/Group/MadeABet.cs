using System.Linq;
using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Commands.MessageHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group
{
    public class MadeABet : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.made_a_bet;

        protected override async Task Execute()
        {
            var repo = new ChosenResultRepository();
            var chatId = await repo.GetAsync(User);
            if (chatId == null)
                await User.Messages.SendMessage(Messages.cant_define_bet);
            else
            {
                await repo.DeleteAsync(User);
                var chat = await Context.TelegramChats.FindById(chatId.Value);
                
                var userStickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
                var userSticker = User.Stickers.SingleOrDefault(item => item.Id == userStickerId);
                if (userSticker == null) return;
                
                var roulette = await Context.ChatRoulette
                    .Where(item => !item.IsStarted && item.Group.Id == chat!.Id)
                    .OrderBy(item => item.Id)
                    .LastOrDefaultAsync();
                
                if (roulette != null) await roulette.MadeABet(userSticker);
                else await chat.SendMessage(string.Format(Messages.roulette_now_ended, MessageCommands.roulette,
                    AppSettings.NAME));
            }
        }
    }
}