using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SendPrivateSticker : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.send_private_sticker;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            var dailyTask = DailyTask.List[DailyTaskKeys.SendStickersToUsers];
            if (await dailyTask.Execute(User.Id))
            {
                await dailyTask.GiveReward(User.Id);
                var message = await MessageController.SendMessage(User, Messages.pack_prize);
                User.Session.Messages.Add(message.MessageId);
            }
        }

        public SendPrivateSticker() { }
        public SendPrivateSticker(UserEntity user, Update update) : base(user, update) { }
    }
}