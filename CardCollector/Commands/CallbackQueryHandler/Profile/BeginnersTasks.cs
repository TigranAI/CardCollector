using System;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    public class BeginnersTasks : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.beginners_tasks;
        protected override async Task Execute()
        {
            var tp = User.InviteInfo!.TasksProgress!;
            var timespan = User.InviteInfo.InvitedAt!.Value + Constants.BEGINNERS_TASKS_INTERVAL - DateTime.Now;

            var message =
                $"{Messages.beginners_tasks} {tp.Progress} / {BeginnersTasksProgress.TaskCount}" +
                $"\n{Messages.open_pack} ({CompleteSelector(tp.OpenPack)})" +
                $"\n{Messages.send_sticker_to_private} ({CompleteSelector(tp.SendStickersToPrivate == BeginnersTasksProgress.SendStickersGoalToPrivate)})" +
                $"\n{Messages.claim_income} ({CompleteSelector(tp.CollectIncome == BeginnersTasksProgress.CollectIncomeGoal)})" +
                $"\n{Messages.buy_standard_pack} ({CompleteSelector(tp.BuyStandardPack)})" +
                $"\n{Messages.combine_stickers} ({CompleteSelector(tp.CombineStickers)})" +
                $"\n{Messages.take_part_at_chat_giveaway} ({CompleteSelector(tp.TakePartAtChatGiveaway)})" +
                $"\n{Messages.play_roulette} ({CompleteSelector(tp.PlayRoulette == BeginnersTasksProgress.PlayRouletteGoal)})" +
                $"\n{Messages.win_roulette} ({CompleteSelector(tp.WinRoulette == BeginnersTasksProgress.WinRouletteGoal)})" +
                $"\n{Messages.buy_sticker_on_auction} ({CompleteSelector(tp.BuyStickerOnAuction)})" +
                $"\n{Messages.place_sticker_on_auction} ({CompleteSelector(tp.PlaceStickerOnAuction)})" +
                $"\n{Messages.donate} ({CompleteSelector(tp.Donate)})" +
                $"\n{Messages.invite_friend} ({CompleteSelector(tp.InviteFriend)})" +
                $"\n{string.Format(Messages.days_left_to_complete, timespan.Days, timespan.Hours, timespan.Minutes)}";

            await User.Messages.EditMessage(User, message, Keyboard.BackKeyboard);
        }

        private string CompleteSelector(bool expression)
        {
            return expression ? Text.complete : Text.uncomplete;
        }
    }
}