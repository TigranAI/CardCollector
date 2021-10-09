using System.Threading.Tasks;
using CardCollector.DailyTasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class SendPrivateSticker : ChosenInlineResult
    {
        protected override string CommandText => Command.send_private_sticker;
        
        public override async Task Execute()
        {
            var dailyTask = DailyTask.List[DailyTaskKeys.SendStickersToUsers];
            if (await dailyTask.Execute(User.Id))
            {
                // TODO give reward
            }
        }

        public SendPrivateSticker() { }
        public SendPrivateSticker(UserEntity user, Update update) : base(user, update) { }
    }
}