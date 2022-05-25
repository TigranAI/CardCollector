using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Cache.Repository;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Games;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.ChosenInlineResultHandler.Group
{
    [Statistics]
    public class ChatSendSticker : ChosenInlineResultHandler
    {
        private static readonly int MAX_EXP_COUNT = Constants.DEBUG ? 10 : 5;
        protected override string CommandText => ChosenInlineResultCommands.chat_send_sticker;

        protected override async Task Execute()
        {
            var repo = new ChosenResultRepository();
            var chatId = await repo.GetAsync(User);
            var data = ChosenInlineResult.ResultId.Split("=");
            var userStickerId = long.Parse(data[1]);

            if (chatId == null)
            {
                if (User.Settings[UserSettingsTypes.ExpGain])
                    await User.Messages.EditMessage($"{Messages.you_gained} 1 {Text.exp} {Messages.send_sticker}" +
                                                    $"\n{Messages.you_can_add_bot_to_conversation}");
                User.Level.GiveExp(1);
            }
            else
            {
                await repo.DeleteAsync(User);
                var chat = await Context.TelegramChats.FindById(chatId.Value);
                await GiveExp(chat!);
                var userSticker = User.Stickers.First(item => item.Id == userStickerId);
                await Ladder.OnStickerReceived(Context, chat!, userSticker.Sticker, User);
            }

            await User.Level.CheckLevelUp(Context);
            User.Stickers.SingleOrDefault(item => item.Id == userStickerId)?.UpdateLastUsage();
        }

        private async Task GiveExp(TelegramChat chat)
        {
            var chatRepo = new ChatInfoRepository();
            var info = await chatRepo.GetAsync(chat);
            if (!info.IsLimitReached(User.Id, MAX_EXP_COUNT))
            {
                var exp = Math.Min(chat.MembersCount, chat.MaxExpGain);
                User.Level.GiveExp(exp);
                if (User.Settings[UserSettingsTypes.ExpGain])
                    await User.Messages.SendMessage(
                        $"{Messages.you_gained} {exp} {Text.exp} {Messages.send_sticker} \"{chat.Title}\"" +
                        $"\n{Messages.today_exp_gain}  {info.GetAndIncrease(User.Id)} / {MAX_EXP_COUNT} {Messages.attempts}");
                await chatRepo.SaveAsync(chat, info);
            }
        }
    }
}