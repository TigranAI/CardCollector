using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class StopBot : MessageCommand
    {
        protected override string CommandText => Text.stop_bot;

        public override async Task Execute()
        {
            await Bot.StopProgram();
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public StopBot() { }
        public StopBot(UserEntity user, Update update) : base(user, update) { }
    }
}