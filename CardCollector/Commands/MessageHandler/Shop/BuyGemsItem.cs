using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.PreCheckoutQueryHandler;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Shop
{
    [Statistics]
    public class BuyGemsItem : MessageHandler
    {
        protected override string CommandText => PreCheckoutQueryCommands.buy_gems_item;

        protected override async Task Execute()
        {
            await User.Messages.ClearChat(User);
            
            var amount = Message.SuccessfulPayment!.TotalAmount;
            var gemsCount = amount * 5 / 100;
            
            await User.AddGems(gemsCount);
            User.Level.GiveExp(gemsCount * 2);
            await User.Level.CheckLevelUp(Context, User);
            
            if (User.Settings[UserSettingsTypes.ExpGain])
                await User.Messages.SendMessage(User,
                    $"{Messages.you_gained} {gemsCount * 2} {Text.exp} {Messages.for_buy_gems}",
                    Keyboard.BackKeyboard);
            
            await User.Messages.SendMessage(User, Messages.thanks_for_buying_gems, Keyboard.BackKeyboard);
            
            if (User.InviteInfo?.TasksProgress is { } tp && !tp.Donate)
            {
                tp.Donate = true;
                await User.InviteInfo.CheckRewards(Context);
            }
        }

        protected override async Task AfterExecute()
        {
            await Context.Payments.Save(User, Message.SuccessfulPayment!);
            await base.AfterExecute();
        }

        public override bool Match()
        {
            return Message.Type == MessageType.SuccessfulPayment &&
                   Message.SuccessfulPayment!.InvoicePayload.Equals(CommandText);
        }
    }
}