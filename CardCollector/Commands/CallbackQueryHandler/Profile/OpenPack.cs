using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class OpenPack : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.open_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var packId = int.Parse(CallbackQuery.Data!.Split("=")[1]);
            var userPack = User.Packs.SingleOrDefault(item => item.Pack.Id == packId);
            if (userPack == null || userPack.Count < 1)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.packs_count_zero, true);
            else
            {
                userPack.Pack.OpenedCount++;
                userPack.Count--;
                var tier = GetTier(Utilities.rnd.NextDouble() * 100);
                var stickers = userPack.Pack.Id != 1
                    ? userPack.Pack.Stickers.Where(sticker => sticker.Tier == tier)
                    : await Context.Stickers.Where(sticker => sticker.Tier == tier).ToListAsync();
                var result = stickers.Random();
                await User.Messages.ClearChat(User);
                await User.Messages.SendSticker(User, result.FileId);
                await User.Messages.SendMessage(User, $"{Messages.congratulation}\n{result}",
                    userPack.Count > 0
                        ? Keyboard.RepeatCommand(Text.open_more, CallbackQuery.Data!)
                        : Keyboard.BackKeyboard);
                await User.AddSticker(Context, result, 1);
            }
        }

        private int GetTier(double chance)
        {
            return chance switch
            {
                < 0.7 => 4,
                < 3.3 => 3,
                < 16 => 2,
                _ => 1
            };
        }

        public OpenPack(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}