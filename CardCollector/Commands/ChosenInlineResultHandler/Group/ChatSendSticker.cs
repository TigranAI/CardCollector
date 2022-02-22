using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.DataBase;
using CardCollector.Resources;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group
{
    [SavedActivity]
    public class ChatSendSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.chat_send_sticker;
        protected override async Task Execute()
        {
            if (!User.Session.ChosenResultWithMessage)
            {
                if (User.Settings[Resources.Enums.UserSettings.ExpGain])
                    await User.Messages.EditMessage(User,
                        $"{Messages.you_gained} 1 {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.you_can_add_bot_to_conversation}");
                User.Level.GiveExp(1);
                await User.Level.CheckLevelUp(Context, User);
            }
            else User.Session.ChosenResultWithMessage = false;
        }

        public ChatSendSticker(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) :
            base(user, context, chosenInlineResult)
        {
        }
    }
}