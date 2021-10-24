using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class Combine : CallbackQueryCommand
    {
        protected override string CommandText => Command.combine;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            var combineModule = User.Session.GetModule<CombineModule>();
            if (User.Session.State != UserState.CombineMenu)
                combineModule.CombineList.Clear();
            User.Session.State = UserState.CombineMenu;
            if (combineModule.SelectedSticker == null)
            {
                var collectionModule = User.Session.GetModule<CollectionModule>();
                combineModule.SelectedSticker = collectionModule.SelectedSticker;
                combineModule.Count = collectionModule.Count;
            }
            var selectedSticker = combineModule.SelectedSticker;
            var combineCount = combineModule.GetCombineCount();
            if (combineCount == Constants.COMBINE_COUNT)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_combine, true);
            else
            {
                if (combineCount + combineModule.Count > Constants.COMBINE_COUNT)
                {
                    combineModule.Count = Constants.COMBINE_COUNT - combineCount;
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, $"{Messages.combine_added_only} " +
                        $"{combineModule.Count}{Text.items}", true);
                }
                if (combineModule.CombineList.ContainsKey(selectedSticker))
                {
                    var maxCount = User.Stickers[selectedSticker.Md5Hash].Count;
                    if (maxCount < combineModule.CombineList[selectedSticker] + combineModule.Count)
                        combineModule.CombineList[selectedSticker] = combineModule.Count;
                    else
                        combineModule.CombineList[selectedSticker] += combineModule.Count;
                }
                else combineModule.CombineList.Add(selectedSticker, combineModule.Count);
            }
            await new CombineMenu(User, Update).PrepareAndExecute();
        }

        public Combine() { }
        public Combine(UserEntity user, Update update) : base(user, update) { }
    }
}