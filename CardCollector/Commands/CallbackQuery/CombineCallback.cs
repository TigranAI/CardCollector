using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineCallback : CallbackQuery
    {
        protected override string CommandText => Command.combine;
        public override async Task Execute()
        {
            User.Session.CombineList.Add(User.Session.SelectedSticker);
            
        }

        public CombineCallback() { }
        public CombineCallback(UserEntity user, Update update) : base(user, update) { }
    }
}