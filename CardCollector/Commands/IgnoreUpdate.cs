using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    /* Данный класс позволяет проигнорировать входящее обновление */
    public class IgnoreUpdate : UpdateModel
    {
        protected override string CommandText => "";
        public override Task Execute() { return  Task.CompletedTask; }
        protected internal override bool IsMatches(UserEntity user, Update update) => true;
    }
}