using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.CallbackQuery
{
    public class ShowTopBy : CallbackQueryCommand
    {
        protected override string CommandText => Command.show_top_by;
        public override async Task Execute()
        {
            var topBy = (TopBy) int.Parse(CallbackData.Split('=')[1]);
            switch (topBy)
            {
                case TopBy.Exp:
                    var usersExp = await UserLevelDao.GetTop(5);
                    var topByExp = Messages.users_top_exp;
                    foreach (var (userLevel, index) in usersExp.WithIndex())
                    {
                        var user = await UserDao.GetById(userLevel.UserId);
                        topByExp += $"\n{index+1}.{user.Username}: {userLevel.TotalExp} {Text.exp}";
                    }
                    User.MessagesId.TopUsersId = 
                        await MessageController.DeleteAndSend(User, User.MessagesId.TopUsersId,
                            topByExp, Keyboard.GetTopButton(TopBy.Tier4Stickers), ParseMode.Html);
                    break;
                case TopBy.Tier4Stickers:
                    var admins = await UserDao
                        .GetAllWhere(user => user._privilegeLevel >= (int)PrivilegeLevel.Programmer);
                    var tier4Stickers = (await StickerDao.GetListWhere(i => i.Tier == 4))
                        .Select(s => s.Md5Hash);
                    var userTier4Stickers = await UserStickerRelationDao
                        .GetListWhere(us => tier4Stickers.Contains(us.ShortHash));
                    var usersTier4StickersCount = userTier4Stickers
                        .GroupBy(i => i.UserId)
                        .Select(i => new {userId = i.Key, count = i.Sum(j=>j.Count)})
                        .OrderByDescending(i => i.count)
                        .Where(item => !admins.Any(user => user.Id == item.userId))
                        .Take(5);
                    var topByTier4 = Messages.users_top_tier_4_stickers_count;
                    foreach (var (idAndCount, index) in usersTier4StickersCount.WithIndex())
                    {
                        var user = await UserDao.GetById(idAndCount.userId);
                        topByTier4 += $"\n{index+1}.{user.Username}: {idAndCount.count} {Text.stickers}";
                    }
                    User.MessagesId.TopUsersId = 
                        await MessageController.DeleteAndSend(User, User.MessagesId.TopUsersId,
                            topByTier4, Keyboard.GetTopButton(TopBy.Exp), ParseMode.Html);
                    break;
            }
        }

        public ShowTopBy()
        {
        }

        public ShowTopBy(UserEntity user, Update update) : base(user, update)
        {
        }
    }
}