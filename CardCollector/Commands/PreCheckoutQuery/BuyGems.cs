using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CardCollector.Commands.PreCheckoutQuery
{
    public class BuyGems : PreCheckoutQueryCommand
    {
        protected override string CommandText => Command.buy_gems_item;

        public override async Task Execute()
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(PreCheckoutQueryId);
            var gemsCount = 50 * Amount / 100;
            User.Cash.Gems += gemsCount;
            await MessageController.EditMessage(User, Messages.thanks_for_buying_gems);
            if (User.Settings[UserSettingsEnum.ExpGain])
                await MessageController.SendMessage(User, 
                    $"{Messages.you_gained} {gemsCount * 2} {Text.exp} {Messages.buy_gems}");
            await User.GiveExp(gemsCount * 2);
        }

        public BuyGems() { }
        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}