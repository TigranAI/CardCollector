using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyGemsQuery : CallbackQuery
    {
        protected override string CommandText => Command.buy_gems;
        public override async Task Execute()
        {
            await User.ClearChat();
            var messages = await MessageController.SendInvoice(User, Text.gems_title, Text.gems_description, 
                Command.gems50, new[] {new LabeledPrice(Text.gems_label50, 100)});
            User.Session.Messages.Add(messages.MessageId);
        }

        public BuyGemsQuery() { }
        public BuyGemsQuery(UserEntity user, Update update) : base(user, update) { }
    }
}