using System.Diagnostics;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace CardCollector.Auction
{
    public static class AuctionController 
    {

        private static async void SellCard(UserEntity user, string hashCode)
        {
            
            var countStikers = user.Stickers[hashCode].Count;
            var price = 0;//устанавливаем сумму за штуку 
            var countForSell = 0;//тут как-то кнопками регулировать количество стикеров для продажи
            if (countForSell > countStikers)
            {
                Debug.Fail("Ты шо, ебобо?");
                //выводим сообщение о ошибке мол у тебя столько нету стикеров
            }
            else
            {
                var summa = price * countForSell;
                //подтверждаем действие
                user.Stickers[hashCode].Count -= countForSell;
                user.Cash.Coins += summa;
            }
        }
        
        private static async void BuyCard()
        {
            
        }
    }
}