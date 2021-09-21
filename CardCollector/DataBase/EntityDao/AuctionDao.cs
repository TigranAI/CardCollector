using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.EntityDao
{
    public static class AuctionDao
    {
        /* Таблица stickers в представлении Entity Framework */
        private static readonly DbSet<AuctionEntity> Table = CardCollectorDatabase.Instance.Auction;


        public static async Task<int> GetProduct(string stickerId)
        {
            int result = 0;
            var products = AuctionDao.Table;

            await foreach (var variable in products)
            {
                if (variable.Id == stickerId)
                {
                    result++;
                }
            }
            return result;
        }

    }
}