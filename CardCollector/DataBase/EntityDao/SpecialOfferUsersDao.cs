using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class SpecialOfferUsersDao
    {
        private static readonly CardCollectorDatabase Instance = CardCollectorDatabase.GetSpecificInstance(typeof(SpecialOfferUsersDao));
        private static readonly DbSet<SpecialOfferUsers> Table = Instance.SpecialOfferUsers;

        public static async Task<bool> NowUsed(long userId, int offerId)
        {
            return await Table.AnyAsync(e => e.UserId == userId && e.OfferId == offerId);
        }

        public static async Task AddNew(long userId, int offerId)
        {
            await Table.AddAsync(new SpecialOfferUsers
            {
                UserId = userId,
                OfferId = offerId
            });
        }
    }
}