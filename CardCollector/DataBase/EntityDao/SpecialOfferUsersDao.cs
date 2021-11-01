using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public class SpecialOfferUsersDao
    {
        public static BotDatabase Instance;
        public static DbSet<SpecialOfferUsers> Table;

        static SpecialOfferUsersDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(SpecialOfferUsersDao));
            Table = Instance.SpecialOfferUsers;
        }
        
        public static async Task<bool> NowUsed(long userId, int offerId)
        {
            try
            {
                return await Table.AnyAsync(e => e.UserId == userId && e.OfferId == offerId);
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                return await NowUsed(userId, offerId);
            }
        }

        public static async Task AddNew(long userId, int offerId)
        {
            try
            {
                await Table.AddAsync(new SpecialOfferUsers
                {
                    UserId = userId,
                    OfferId = offerId
                });
                await BotDatabase.SaveData();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                await AddNew(userId, offerId);
            }
        }
    }
}