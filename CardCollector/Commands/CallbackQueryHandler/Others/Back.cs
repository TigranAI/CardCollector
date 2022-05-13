using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.MessageHandler.Admin.Distribution;
using CardCollector.Commands.MessageHandler.Admin.Giveaway;
using CardCollector.Commands.MessageHandler.Collection;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.Commands.MessageHandler.Shop;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    [SkipCommand]
    public class Back : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.back;

        protected override async Task Execute()
        {
            ClearQueues();

            var command = User.Session.GetPreviousCommand();
            if (command != null)
            {
                await command.InitNewContext(User.Id);
                await command.PrepareAndExecute();
            }
            else
            {
                User.Session.EndSession();
                await User.Messages.ClearChat(User);
                await User.Messages.SendMenu(User);
            }
        }

        private void ClearQueues()
        {
            EnterEmoji.RemoveFromQueue(User.Id);
            EnterButtonText.RemoveFromQueue(User.Id);
            EnterButtonUrl.RemoveFromQueue(User.Id);
            EnterDistributionText.RemoveFromQueue(User.Id);
            EnterGemsExchange.RemoveFromQueue(User.Id);
            EnterGemsPrice.RemoveFromQueue(User.Id);
            EnterGiveawayMessage.RemoveFromQueue(User.Id);
            EnterPrizeCount.RemoveFromQueue(User.Id);
            EnterSendDatetime.RemoveFromQueue(User.Id);
            EnterDistributionButtonName.RemoveFromQueue(User.Id);
        }
    }
}