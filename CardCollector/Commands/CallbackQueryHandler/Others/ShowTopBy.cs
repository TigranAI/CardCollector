using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    public class ShowTopBy : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_top_by;
        protected override async Task Execute()
        {
            var topBy = (TopBy) int.Parse(CallbackQuery.Data!.Split('=')[1]);
            switch (topBy)
            {
                case TopBy.Exp:
                    var usersExpTop = await Context.Users.FindTopByExp();
                    var topByExp = Messages.users_top_exp + string.Join("\n", usersExpTop.Select((user, i) =>
                        $"\n{i + 1}.{user.Username}: {user.Level.TotalExp} {Text.exp}"));
                    await User.Messages.SendTopUsers(User, topByExp, Keyboard.GetTopButton(TopBy.Tier4Stickers));
                    break;
                case TopBy.Tier4Stickers:
                    var usersTier4Top = await Context.Users.FindTopByTier4Stickers();
                    var topByTier4 = Messages.users_top_tier_4_stickers_count + string.Join("\n", 
                        usersTier4Top.Select((user, i) =>
                        {
                            var count = user.Stickers.Where(sticker => sticker.Sticker.Tier == 4).Count();
                            return $"\n{i + 1}.{user.Username}: {count} {Text.stickers}";
                        }));
                    await User.Messages.SendTopUsers(User, topByTier4, Keyboard.GetTopButton(TopBy.Exp));
                    break;
            }
        }
    }
}