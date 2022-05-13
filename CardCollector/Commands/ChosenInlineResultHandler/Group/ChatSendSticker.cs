using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Cache.Entity;
using CardCollector.Cache.Repository;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot;

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
            var chatId = await repo.GetOrDefaultAsync(User.Id);
            if (User.Settings[UserSettingsTypes.ExpGain])
            {
                if (chatId == null)
                {
                    await User.Messages.EditMessage(User,
                        $"{Messages.you_gained} 1 {Text.exp} {Messages.send_sticker}" +
                        $"\n{Messages.you_can_add_bot_to_conversation}");
                    User.Level.GiveExp(1);
                }
                else
                {
                    await repo.DeleteAsync(User.Id);
                    var chat = await Context.TelegramChats.FindByChatId(chatId.Value);
                    await GiveExp(chat);
                }
            }

            await User.Level.CheckLevelUp(Context, User);

            var userStickerId = int.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            User.Stickers.SingleOrDefault(item => item.Id == userStickerId)?.UpdateLastUsage();
        }

        private async Task GiveExp(TelegramChat chat)
        {
            var chatRepo = new ChatInfoRepository();
            var info = await chatRepo.GetOrDefaultAsync(chat.ChatId, new ChatInfo());
            if (!info!.IsLimitReached(User.Id, MAX_EXP_COUNT))
            {
                var exp = Math.Min(chat.MembersCount, chat.MaxExpGain);
                User.Level.GiveExp(exp);
                await User.Messages.SendMessage(User,
                    $"{Messages.you_gained} {exp} {Text.exp} {Messages.send_sticker} \"{chat.Title}\"" +
                    $"\n{Messages.today_exp_gain}  {info.GetAndIncrease(User.Id)} / {MAX_EXP_COUNT} {Messages.attempts}");
                await chatRepo.SaveAsync(chat.ChatId, info);
            }
        }
    }
}