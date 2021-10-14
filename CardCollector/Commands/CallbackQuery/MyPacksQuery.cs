using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class MyPacksQuery : CallbackQuery
    {
        protected override string CommandText => Command.my_packs;
        
        public override async Task Execute()
        {
            await User.ClearChat();
            var userPacks = await UsersPacksDao.GetUserPacks(User.Id);
            var randomCount = (await UsersPacksDao.GetPackInfo(User.Id, 0)).Count;
            var authorCount = userPacks.Sum(pack => pack.PackId != 0 ? pack.Count : 0);
            var message = await MessageController.SendMessage(User, 
                $"{Messages.your_packs}" +
                $"\n{Messages.random_packs} {randomCount}{Text.items}" +
                $"\n{Messages.author_packs} {authorCount}{Text.items}",
                Keyboard.PackMenu);
            User.Session.Messages.Add(message.MessageId);
        }

        public MyPacksQuery() { }
        public MyPacksQuery(UserEntity user, Update update) : base(user, update) { }
    }
}