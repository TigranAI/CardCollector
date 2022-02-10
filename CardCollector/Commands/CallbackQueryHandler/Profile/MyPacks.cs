using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class MyPacks : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.my_packs;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var random = User.Packs.SingleOrDefault(item => item.Pack.Id == 1);
            var authorCount = User.Packs.Sum(item => item.Count) - random?.Count;
            await User.Messages.EditMessage(User, 
                $"{Messages.your_packs}" +
                $"\n{Messages.random_packs}: {random?.Count ?? 0}{Text.items}" +
                $"\n{Messages.author_pack}: {authorCount ?? 0}{Text.items}",
                Keyboard.PackMenu);
        }

        public MyPacks(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}