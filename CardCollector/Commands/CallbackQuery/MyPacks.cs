using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class MyPacks : CallbackQueryCommand
    {
        protected override string CommandText => Command.my_packs;
        protected override bool AddToStack => true;

        public override async Task Execute()
        {
            var random = await UserPacksDao.GetOne(User.Id, 1);
            var authorCount = (await UserPacksDao.GetUserPacks(User.Id)).Sum(item => item.PackId != 1 ? item.Count : 0);
            await MessageController.SendMessage(User, 
                $"{Messages.your_packs}" +
                $"\n{Messages.random_packs}: {random.Count}{Text.items}" +
                $"\n{Messages.author_pack}: {authorCount}{Text.items}",
                Keyboard.PackMenu);
        }

        public MyPacks() { }
        public MyPacks(UserEntity user, Update update) : base(user, update) { }
    }
}