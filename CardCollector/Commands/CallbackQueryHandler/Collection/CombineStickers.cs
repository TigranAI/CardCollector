using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class CombineStickers : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.combine_stickers;

        protected override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            var price = combineModule.CombinePrice;
            if (price == null) return;
            if (User.Cash.Coins < price)
                await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, Messages.not_enougth_coins, true);
            else
            {
                User.Cash.Coins -= price.Value;
                foreach (var (sticker, count) in combineModule.CombineList)
                {
                    var userSticker = User.Stickers.FirstOrDefault(item => item.Sticker.Id == sticker.Id);
                    if (userSticker == null) continue;
                    userSticker.Count -= count;
                }

                var authors = combineModule.CombineList.Select(i => i.Item1.Author).Distinct().ToList();
                var randomAuthor = authors.Random();
                var stickers =
                    await Context.Stickers.FindAllByTierAndAuthor(combineModule.CombineTier!.Value + 1, randomAuthor);
                var randSticker = stickers.Random();
                await User.AddSticker(randSticker, 1);
                await User.Messages.SendSticker(User, randSticker.FileId);
                await User.Messages.EditMessage(User, $"{Messages.combined_sticker}:\n{randSticker}",
                    Keyboard.BackToFilters(randSticker.Title));
                User.Session.DeleteModule<CombineModule>();
            }
        }

        public CombineStickers(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}