using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyGems : CallbackQueryCommand
    {
        protected override string CommandText => Command.buy_gems;
        public override async Task Execute()
        {
            await User.ClearChat();
            var messages = await MessageController.SendInvoice(User, Text.gems_title, Text.gems_description, 
                Command.buy_gems_item, new[] {new LabeledPrice(Text.gems_label50, 100)},
                1000000, new [] {500, 1000, 2500, 5000});
            User.Session.Messages.Add(messages.MessageId);
        }

        public BuyGems() { }
        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}