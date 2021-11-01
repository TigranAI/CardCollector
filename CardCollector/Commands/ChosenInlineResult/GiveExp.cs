using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    /* Данная команда выполняется при отправке пользователем стикера */
    public class GiveExp : ChosenInlineResultCommand
    {
        /* Ключевое слово для данной команды send_sticker */
        protected override string CommandText => Command.give_exp;

        public override async Task Execute()
        {
            if (!User.Session.ChosenResultWithMessage)
            {
                if (User.Settings[UserSettingsEnum.ExpGain])
                    await MessageController.SendMessage(User, 
                        $"{Messages.you_gained} 1 {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.you_can_add_bot_to_conversation}");
                await User.GiveExp(1);
            }
            User.Session.ChosenResultWithMessage = false;
        }
        
        public GiveExp() { }
        public GiveExp(UserEntity user, Update update) : base(user, update) { }
    }
}