using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public abstract class UpdateModel
    {
        protected abstract string Command { get; }
        protected UserEntity User;
        protected Update Update;

        public abstract Task Execute();

        protected virtual bool IsMatches(string command)
        {
            return command.Contains(Command);
        }

        protected UpdateModel()
        {
            User = null;
        }

        protected UpdateModel(UserEntity user, Update update)
        {
            User = user;
            Update = update;
        }
    }
}