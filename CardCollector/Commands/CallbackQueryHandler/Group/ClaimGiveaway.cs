using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Cache.Repository;
using CardCollector.Controllers;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    [Statistics]
    public class ClaimGiveaway : CallbackQueryHandler
    {
        /* syntax command=<chatId>=<prizeType>=<prizeId> */
        protected override string CommandText => CallbackQueryCommands.group_claim_giveaway;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split("=");
            var chat = await Context.TelegramChats.FindById(long.Parse(data[1]));

            var listRepo = new ListRepository<long>();
            
            if (!await listRepo.ContainsAsync(CommandText, chat!.Id)) return;
            if (!await CheckCanBeAwarded(chat.Id)) return;

            await GivePrize(data, chat);
            await listRepo.RemoveAsync(CommandText, chat.Id);

            foreach (var member in chat.Members)
            {
                if (member.InviteInfo?.TasksProgress is { } tp && !tp.TakePartAtChatGiveaway)
                {
                    tp.TakePartAtChatGiveaway = true;
                    await member.InviteInfo.CheckRewards(Context);
                }
            }
        }

        private async Task<bool> CheckCanBeAwarded(long chatId)
        {
            var lastUserAward = User.AvailableChats.FirstOrDefault(item =>
            {
                if (item.LastGiveaway == null) return false;
                if (item.Id == chatId) return false;
                var interval = DateTime.Now - item.LastGiveaway.Value;
                return interval.TotalMinutes < item.GiveawayDuration;
            });
            if (lastUserAward != null)
            {
                var interval = DateTime.Now - lastUserAward.LastGiveaway!.Value;
                await AnswerCallbackQuery(User, CallbackQuery.Id,
                    string.Format(Messages.you_are_now_be_awarded_in_another_group, 
                        lastUserAward.GiveawayDuration - (int) interval.TotalMinutes),
                    true);
                return false;
            }
            return true;
        }

        private async Task GivePrize(string[] data, TelegramChat chat)
        {
            var prizeId = long.Parse(data[3]);

            var prizeMessage = data[2] is "pack"
                ? await ClaimPack(prizeId)
                : data[2] is "sticker"
                    ? await ClaimSticker(prizeId)
                    : "";

            await chat.EditMessage(string.Format(Messages.user_claim_giveaway, User.Username, prizeMessage,
                chat.GiveawayDuration), CallbackQuery.Message!.MessageId);
            User.UserStats.IncreaseGiftsReceived();
        }

        private async Task<string> ClaimSticker(long stickerId)
        {
            var sticker = await Context.Stickers.FindById(stickerId);
            await User.AddSticker(sticker, 1);
            return $"{sticker.Title} {sticker.TierAsStars()}";
        }

        private async Task<string> ClaimPack(long packId)
        {
            var pack = await Context.Packs.FindById((int) packId);
            User.AddPack(pack, 1);
            return pack.Author;
        }
    }
}