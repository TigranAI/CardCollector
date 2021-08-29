using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Commands.MessageCommands;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public abstract class UpdateModel
    {
        protected abstract string Command { get; }
        protected UserEntity User;

        public abstract Task<Message> Execute();

        protected virtual bool IsMatches(string command)
        {
            return command.Contains(Command);
        }
    }
}