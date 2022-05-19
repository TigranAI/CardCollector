using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Controllers;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Group
{
    [Statistics]
    public class Roulette : MessageHandler
    {
        protected override string CommandText => MessageCommands.roulette;
        protected override async Task Execute()
        {
            var chat = await Context.TelegramChats.FindByChat(Message.Chat);
            if (Context.ChatRoulette.Any(item => !item.IsStarted && item.Group.Id == chat.Id))
            {
                await chat.SendMessage(Messages.roulette_start_now);
                return;
            }
            var roulette = new ChatRoulette()
            {
                Creator = User,
                Group = chat
            };
            var entityResult = await Context.ChatRoulette.AddAsync(roulette);
            roulette = entityResult.Entity;
            await Context.SaveChangesAsync();
            var messageId = await chat.SendMessage(string.Format(Messages.roulette_message, User.Username, ""),
                Keyboard.RouletteKeyboard(roulette.Id));
            roulette.MessageId = messageId;
            
            TimerController.SetupTimer(Constants.ROULETTE_INTERVAL, roulette.Start);
        }

        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            var data = Message.Text!.Split("@");
            if (data.Length < 2) return false;
            if (data[0] != CommandText || data[1] != AppSettings.NAME) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }
    }
}