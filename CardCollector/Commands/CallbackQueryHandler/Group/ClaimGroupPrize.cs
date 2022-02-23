using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    [SavedActivity]
    public class ClaimGroupPrize : CallbackQueryHandler
    {
        /* syntax command=<chatId>=<prizeType>=<prizeId> */
        protected override string CommandText => CallbackQueryCommands.claim_group_prize;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split("=");
            var chat = await Context.TelegramChats.FindById(int.Parse(data[1]));
            
            if (await CheckPrizeIsClaimed(chat)) return;
            if (!await CheckCanBeAwarded()) return;
            
            await GivePrize(data, chat);
            UpdateChatInfo(chat);
        }

        private static void UpdateChatInfo(TelegramChat? chat)
        {
            chat.ChatActivity!.MessageCountAtLastGiveaway = chat.ChatActivity.MessageCount;
            chat.ChatActivity.LastGiveaway = DateTime.Now;
            chat.ChatActivity.PrizeClaimed = true;
            
        }

        private async Task<bool> CheckPrizeIsClaimed(TelegramChat? chat)
        {
            if (chat.ChatActivity.PrizeClaimed)
            {
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.prize_now_claimed);
                return true;
            }
            return false;
        }

        private async Task<bool> CheckCanBeAwarded()
        {
            var userGroups = await Context.TelegramChats
                .Where(telegramChat => telegramChat.Members.Contains(User))
                .ToListAsync();
            var userCantBeAwarded = userGroups.Any(item =>
            {
                var interval = DateTime.Now - item.ChatActivity.LastGiveaway;
                return interval.TotalHours < GroupController.GROUP_GIVEAWAY_HOURS_INTERVAL;
            });
            if (userCantBeAwarded)
            {
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, 
                    Messages.you_are_now_be_awarded_in_another_group);
                return false;
            }
            return true;
        }

        private async Task GivePrize(string[] data, TelegramChat? chat)
        {
            var prizeId = long.Parse(data[3]);

            var prizeMessage = data[2] is "pack"
                ? await ClaimPack(prizeId)
                : data[2] is "sticker"
                    ? await ClaimSticker(prizeId)
                    : "";

            await chat!.EditMessage(string.Format(Messages.user_claim_giveaway, User.Username, prizeMessage,
                GroupController.GROUP_GIVEAWAY_HOURS_INTERVAL), CallbackQuery.Message!.MessageId);
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

        public ClaimGroupPrize(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}