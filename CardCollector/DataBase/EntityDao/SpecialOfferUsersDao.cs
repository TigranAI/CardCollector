using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class SpecialOfferUsersDao
    {
        public static async Task<bool> NowUsed(long userId, int offerId)
        {
            var Table = BotDatabase.Instance.SpecialOfferUsers;
            return await Table.AnyAsync(e => e.UserId == userId && e.OfferId == offerId);
        }

        public static async Task AddNew(long userId, int offerId)
        {
            var Table = BotDatabase.Instance.SpecialOfferUsers;
            await Table.AddAsync(new SpecialOfferUsers
            {
                UserId = userId,
                OfferId = offerId
            });
            await BotDatabase.SaveData();
        }
    }
}