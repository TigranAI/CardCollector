using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
            while (module.SelectBetChatId == null)
                await new Task(() => Thread.Sleep(100));

            var roulette = await Context.ChatRoulette
                .Where(item => !item.IsStarted && item.Group.ChatId == module.SelectBetChatId)
                .OrderBy(item => item.Id)
                .LastOrDefaultAsync();
            if (roulette != null) await roulette.MadeABet(userSticker);
            else
            {
                var telegramChat = await Context.TelegramChats.FindByChatId(module.SelectBetChatId.Value);
                if (telegramChat == null) return;
                await telegramChat.SendMessage(string.Format(Messages.roulette_now_ended, MessageCommands.roulette,
                    AppSettings.NAME));
            }
        }

        public MadeABet(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(user,
            context, chosenInlineResult)
        {
        }
    }
}