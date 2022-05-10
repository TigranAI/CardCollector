using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group
{
    public class MadeABet : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.made_a_bet;

        protected override async Task Execute()
        {
            var userStickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            var userSticker = User.Stickers.SingleOrDefault(item => item.Id == userStickerId);
            if (userSticker == null) return;

            var module = User.Session.GetModule<GroupModule>();
            if (module.SelectBetChatId == null) await User.Messages.SendMessage(User, Messages.cant_define_bet);

            var roulette = await Context.ChatRoulette
                .Where(item => !item.IsStarted && item.Group.ChatId == module.SelectBetChatId)
                .OrderBy(item => item.Id)
                .LastOrDefaultAsync();
            if (roulette != null) await roulette.MadeABet(userSticker);
            else
            {
                var telegramChat = await Context.TelegramChats.FindByChatId(module.SelectBetChatId);
                if (telegramChat == null) return;
                await telegramChat.SendMessage(string.Format(Messages.roulette_now_ended, MessageCommands.roulette,
                    AppSettings.NAME));
            }
        }
    }
}