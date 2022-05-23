using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Sticker = CardCollector.Database.Entity.Sticker;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class Combine : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.combine;

        protected override async Task Execute()
        {
            User.Session.State = UserState.CombineMenu;
            var combineModule = User.Session.GetModule<CombineModule>();
            if (combineModule.SelectedStickerId == null)
            {
                var collectionModule = User.Session.GetModule<CollectionModule>();
                combineModule.SelectedStickerId = collectionModule.SelectedStickerId;
                combineModule.Count = collectionModule.Count;
            }
            var selectedSticker = User.Stickers.Single(item => item.Id == combineModule.SelectedStickerId);
            var combineCount = combineModule.CombineCount;
            if (combineCount == Constants.COMBINE_COUNT)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.cant_combine, true);
            else
            {
                if (combineCount + combineModule.Count > Constants.COMBINE_COUNT)
                {
                    combineModule.Count = Constants.COMBINE_COUNT - combineCount;
                    await AnswerCallbackQuery(User, CallbackQuery.Id, $"{Messages.combine_added_only} " +
                        $"{combineModule.Count}{Text.items}", true);
                }
                combineModule.CombineList.Add(new Tuple<Sticker, int>(selectedSticker.Sticker, combineModule.Count));
            }

            await new CombineMenu().Init(User, Context, new Update() {CallbackQuery = CallbackQuery}).PrepareAndExecute();
        }
    }
}