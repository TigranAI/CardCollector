using System.Linq;
using System.Threading.Tasks;
using CardCollector.Cache.Entity;
using CardCollector.Cache.Repository;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Group
{
    public class ClaimLadder : CallbackQueryHandler
    {
        private static readonly int MAX_LADDER_PRIZES = Constants.DEBUG ? 1 : 2;
        protected override string CommandText => CallbackQueryCommands.group_claim_ladder;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split("=");
            var chatId = int.Parse(data[1]);
            var listRepo = new ListRepository<long>();
            if (!await listRepo.ContainsAsync(CommandText, chatId)) return;

            var repo = new UserInfoRepository();
            var info = await repo.GetOrDefaultAsync(User.Id, new UserInfo());
            if (info!.TryClaimLadder(MAX_LADDER_PRIZES))
            {
                var chat = await Context.TelegramChats.FindById(chatId);
                var packId = int.Parse(data[2]);
                var pack = await Context.Packs.FindById(packId);
                await chat.EditMessage(string.Format(Messages.ladder_message_claimed,
                        User.Username, pack.Author),
                    CallbackQuery.Message.MessageId);
                User.AddPack(pack, 1);
                await repo.SaveAsync(User.Id, info);
                await User.Stickers
                    .Where(sticker => sticker.Sticker.ExclusiveTask is ExclusiveTask.ClaimLadderPrize)
                    .Apply(sticker => sticker.DoExclusiveTask());
            }
            else
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.you_are_now_reach_limit);
        }
    }
}