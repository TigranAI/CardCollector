using System.Threading.Tasks;
using System.Timers;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Admin
{
    public class StopBot : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.stop_bot;
        public static bool ConfirmStop;

        protected override async Task Execute()
        {
            if (ConfirmStop) await Bot.StopProgram();
            else
            {
                await User.Messages.EditMessage(User, Messages.confirm_stopping, Keyboard.StopKeyboard);
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

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}