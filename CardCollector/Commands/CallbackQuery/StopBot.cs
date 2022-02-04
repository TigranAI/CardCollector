using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class StopBot : CallbackQueryHandler
    {
        protected override string CommandText => Command.stop_bot;
        public static bool ConfirmStop = false;

        public override async Task Execute()
        {
            if (ConfirmStop) await Bot.StopProgram();
            else
            {
                await MessageController.EditMessage(User, Messages.confirm_stopping, Keyboard.StopKeyboard);
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

        protected internal override bool Match(UserEntity user, Update update)
        {
            return base.Match(user, update) && user.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public StopBot(UserEntity user, Update update) : base(user, update) { }
    }
}