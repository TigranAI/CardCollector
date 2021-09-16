using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class SelectAuthorCallback : CallbackQuery
    {
        protected override string Command => CallbackQueryCommands.author_callback;
        public override async Task Execute()
        {
            var result = CallbackData.Split('=')[1];
            User.Filters["author"] = result;
            await new BackToFiltersMenu(User, Update).Execute();
        }
        
        public SelectAuthorCallback() { }
        public SelectAuthorCallback(UserEntity user, Update update) : base (user, update) { }
    }
}