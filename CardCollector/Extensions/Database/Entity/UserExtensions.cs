using System.Collections.Generic;
using CardCollector.Database.Entity;
using CardCollector.Resources.Translations;

namespace CardCollector.Extensions.Database.Entity
{
    public static class UserExtensions
    {
        public static string GetProfileMessage(this User user, string expGoal)
        {
            List<string> rows = new();
            rows.Add($"{user.Username}");
            rows.Add($"{Messages.coins}: {user.Cash.Coins}{Text.coin}");
            rows.Add($"{Messages.gems}: {user.Cash.Gems}{Text.gem}");
            rows.Add($"{Messages.candies}: {user.Cash.Candies}{Text.candy}");
            rows.Add($"{Messages.level}: {user.Level.Level}");
            rows.Add($"{Messages.current_exp}: {user.Level.CurrentExp} / {expGoal}");
            rows.Add($"{Messages.cash_capacity}: {user.Cash.MaxCapacity}{Text.coin}");
            if (user.InviteInfo?.ShowInvitedBy() is true)
                rows.Add($"{Messages.inviter}: {user.InviteInfo.InvitedBy!.Username}");
            rows.Add($"{Messages.see_your_stickers}");
            return string.Join("\n", rows);
        }
    }
}