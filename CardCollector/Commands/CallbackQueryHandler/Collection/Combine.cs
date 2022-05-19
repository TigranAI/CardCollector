using System;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Collection
{
    public class Combine : CombineMenu
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
            var selectedSticker = await Context.Stickers.FindById(combineModule.SelectedStickerId!.Value);
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
                combineModule.CombineList.Add(new Tuple<Sticker, int>(selectedSticker, combineModule.Count));
            }
            await base.Execute();
        }
    }
}