using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.Message
{
    /* Команда "Профиль" Отображает профиль пользователя и его баланс */
    public class Profile : MessageCommand
    {
        /* Для данной команды ключевое слово "Профиль" */
        protected override string CommandText => Text.profile;
        protected override bool ClearMenu => true;
        protected override bool AddToStack => true;
        protected override bool ClearStickers => true;

        public override async Task Execute()
        {
            /* Подсчитываем прибыль */
            var income = await User.Cash.CalculateIncome(User.Stickers);
            var expGoal = (await LevelDao.GetLevel(User.CurrentLevel.Level + 1))?.LevelExpGoal.ToString() ?? "∞";
            var packsCount = await UserPacksDao.GetCount(User.Id);
            /* Отправляем сообщение */
            await MessageController.EditMessage(User, 
                $"{User.Username}" +
                $"\n{Messages.coins}: {User.Cash.Coins}{Text.coin}" +
                $"\n{Messages.gems}: {User.Cash.Gems}{Text.gem}" +
                $"\n{Messages.level}: {User.CurrentLevel.Level}" +
                $"\n{Messages.current_exp}: {User.CurrentLevel.CurrentExp} / {expGoal}" +
                $"\n{Messages.cash_capacity}: {User.Cash.MaxCapacity}{Text.coin}" +
                $"\n{Messages.see_your_stickers}",
                Keyboard.GetProfileKeyboard(packsCount, income));
        }
        
        public Profile() { }
        public Profile(UserEntity user, Update update) : base(user, update) { }
    }
}