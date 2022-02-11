using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.Others;
using CardCollector.Resources;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
                    ? userPack.Pack.Stickers
                    : await Context.Stickers.ToListAsync();
                var result = stickers.Where(sticker => sticker.Tier == tier).Random();
                await User.AddSticker(result, 1);
                await User.Messages.SendSticker(User, result.FileId);
                await User.Messages.EditMessage(User, $"{Messages.congratulation}\n{result}",
                    userPack.Count > 0
                        ? Keyboard.RepeatCommand(Text.open_more, CallbackQuery.Data!)
                        : Keyboard.BackKeyboard);
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