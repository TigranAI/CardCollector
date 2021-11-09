using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    public class StopBot : MessageCommand
    {
        protected override string CommandText => Text.stop_bot;
        public static bool ConfirmStop = false;

        public override async Task Execute()
        {
            if (ConfirmStop) await Bot.StopProgram();
            else
            {
                await MessageController.EditMessage(User, Messages.confirm_stopping);
                ConfirmStop = true;
                var timer = new Timer
                {
                    AutoReset = false,
                    Enabled = true,
                    Interval = 60 * 1000
                };
                timer.Elapsed += (_, _) => ConfirmStop = false;
            }
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return base.IsMatches(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public StopBot() { }
        public StopBot(UserEntity user, Update update) : base(user, update) { }
    }
}