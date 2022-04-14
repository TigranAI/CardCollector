using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group
{
    [SavedActivity]
    public class ChatSendSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.chat_send_sticker;
        protected override async Task Execute()
        {
            var stickerId = int.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            User.Stickers.First(item => item.Sticker.Id == stickerId).LastUsage = DateTime.Now;
            
            if (!User.Session.ChosenResultWithMessage)
            {
                if (User.Settings[UserSettingsTypes.ExpGain])
                    await User.Messages.EditMessage(User,
                        $"{Messages.you_gained} 1 {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.you_can_add_bot_to_conversation}");
                User.Level.GiveExp(1);
                await User.Level.CheckLevelUp(Context, User);
            }
            else User.Session.ChosenResultWithMessage = false;
        }
    }
}