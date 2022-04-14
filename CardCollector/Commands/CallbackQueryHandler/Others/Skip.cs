using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Admin.Distribution;
using CardCollector.Commands.MessageHandler.Admin.Giveaway;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class Skip : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.skip;
        protected override async Task Execute()
        {
            var commandName = CallbackQuery.Data!.Split("=")[1];
            if (commandName == typeof(EnterSendDatetime).Name) await EnterSendDatetime.Skip(User, Context);
            else if (commandName == typeof(EnterButtonText).Name) await EnterButtonText.Skip(User, Context);
            else if (commandName == typeof(SendGiveawayImage).Name) await SendGiveawayImage.Skip(User, Context);
            else if (commandName == typeof(SendDistributionImage).Name) await SendDistributionImage.Skip(User, Context);
            else if (commandName == typeof(SendDistributionSticker).Name) await SendDistributionSticker.Skip(User, Context);
        }
    }
}