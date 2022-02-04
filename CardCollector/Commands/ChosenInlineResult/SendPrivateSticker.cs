using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SendPrivateSticker : SendStickerHandler
    {
        protected override string CommandText => Command.send_private_sticker;

        public override async Task Execute()
        {
            var dailyTask = DailyTask.List[DailyTaskKeys.SendStickersToUsers];
            if (await dailyTask.Execute(User))
            {
                await dailyTask.GiveReward(User);
                User.MessagesId.DailyTaskProgressId =
                    await MessageController.DeleteAndSend(User, User.MessagesId.DailyTaskProgressId,
                        Messages.pack_prize, Keyboard.MyPacks);
            }
        }

        public SendPrivateSticker(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}