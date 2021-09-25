using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineCallback : CallbackQuery
    {
        protected override string CommandText => Command.combine;
        public override async Task Execute()
        {
            var selectedSticker = User.Session.SelectedSticker;
            var combineCount = User.Session.GetCombineCount();
            if (combineCount == Constants.COMBINE_COUNT)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_combine, true);
            else
            {
                if (combineCount + selectedSticker.Count > Constants.COMBINE_COUNT)
                {
                    selectedSticker.Count = Constants.COMBINE_COUNT - combineCount;
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, $"{Messages.combine_added_only} " +
                        $"{selectedSticker.Count}{Text.items}", true);
                }
                if (User.Session.CombineList.ContainsKey(selectedSticker.Md5Hash))
                {
                    var combineSticker = User.Session.CombineList[selectedSticker.Md5Hash];
                    if (selectedSticker.MaxCount < combineSticker.Count + selectedSticker.Count)
                        User.Session.CombineList[selectedSticker.Md5Hash].Count = selectedSticker.Count;
                    else
                        User.Session.CombineList[selectedSticker.Md5Hash].Count += selectedSticker.Count;
                }
                else User.Session.CombineList.Add(selectedSticker.Md5Hash, selectedSticker);
            }
            await new BackToCombine(User, Update).Execute();
        }

        public CombineCallback() { }
        public CombineCallback(UserEntity user, Update update) : base(user, update) { }
    }
}