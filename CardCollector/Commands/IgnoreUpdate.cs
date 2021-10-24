using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    /* Данный класс позволяет проигнорировать входящее обновление */
    public class IgnoreUpdate : UpdateModel
    {
        protected override string CommandText => "";
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override Task Execute() { return  Task.CompletedTask; }
        protected internal override bool IsMatches(UserEntity user, Update update) => true;
    }
}