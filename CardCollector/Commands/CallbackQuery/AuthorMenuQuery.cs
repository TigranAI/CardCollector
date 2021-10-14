using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class AuthorMenuQuery : CallbackQuery
    {
        protected override string CommandText => Command.author_menu;
        public override async Task Execute()
        {
            
        }

        public AuthorMenuQuery() { }
        public AuthorMenuQuery(UserEntity user, Update update) : base(user, update) { }
    }
}