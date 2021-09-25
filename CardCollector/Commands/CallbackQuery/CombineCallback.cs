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
            var combineCount = User.Session.GetCombineCount();
            if (combineCount == Constants.COMBINE_COUNT)
                await MessageController.AnswerCallbackQuery(User, CallbackQueryId, Messages.cant_combine, true);
            else
            {
                if (combineCount + User.Session.SelectedSticker.Count > Constants.COMBINE_COUNT)
                {
                    User.Session.SelectedSticker.Count = Constants.COMBINE_COUNT - combineCount;
                    await MessageController.AnswerCallbackQuery(User, CallbackQueryId, $"{Messages.combine_added_only} " +
                        $"{User.Session.SelectedSticker.Count}{Text.items}", true);
                }

                if (!User.Session.CombineList.ContainsKey(User.Session.SelectedSticker.Md5Hash))
                    User.Session.CombineList.Add(User.Session.SelectedSticker.Md5Hash, User.Session.SelectedSticker);
                else
                    User.Session.CombineList[User.Session.SelectedSticker.Md5Hash].Count = User.Session.SelectedSticker.Count;
            }
            await new BackToCombine(User, Update).Execute();
        }

        public CombineCallback() { }
        public CombineCallback(UserEntity user, Update update) : base(user, update) { }
    }
}