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
        
        public override async Task Execute()
        {
            await User.ClearChat();
            var userPack = await UsersPacksDao.GetUserPacks(User.Id);
            var specificCount = await SpecificPackDao.GetCount(User.Id);
            var message = await MessageController.SendMessage(User, 
                $"{Messages.your_packs}" +
                $"\n{Messages.random_packs}: {userPack.RandomCount}{Text.items}" +
                $"\n{Messages.author_packs}: {userPack.AuthorCount}{Text.items}" +
                $"\n{Messages.specific_packs} {specificCount}{Text.items}",
                Keyboard.PackMenu);
            User.Session.Messages.Add(message.MessageId);
        }

        public MyPacks() { }
        public MyPacks(UserEntity user, Update update) : base(user, update) { }
    }
}