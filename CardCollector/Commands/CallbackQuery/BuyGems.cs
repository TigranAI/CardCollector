using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class BuyGems : CallbackQueryHandler
    {
        protected override string CommandText => Command.buy_gems;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            module.SelectedProvider = CallbackData.Split("=")[1];
            await MessageController.EditMessage(User, Messages.buy_gems, Keyboard.BuyGems);
        }

        protected internal override bool Match(UserEntity user, Update update)
        {
            return base.Match(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public BuyGems(UserEntity user, Update update) : base(user, update) { }
    }
}