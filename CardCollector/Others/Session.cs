using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;

namespace CardCollector.Others
{
    public class UserSession : IDisposable
    {
        private DateTime _lastAccess = DateTime.Now;

        private readonly UserEntity user;

        public UserSession(UserEntity user)
        {
            this.user = user;
        }

        /* Текущее состояние пользователя */
        public UserState State = UserState.Default;
        
        /* Выбранный пользователем стикер для покупки (продажи, слияния) */
        public StickerInfo SelectedSticker { get; set; }

        public void UpdateLastAccess()
        {
            _lastAccess = DateTime.Now;
        }

        public int GetLastAccessInterval()
        {
            return (int) (DateTime.Now - _lastAccess).TotalMinutes;
        }

        /* Фильтры, примененные пользователем в меню коллекции/магазина/аукциона */
        public readonly Dictionary<string, object> Filters = new()
        {
            {Command.author, ""},
            {Command.tier, -1},
            {Command.emoji, ""},
            {Command.price_coins_from, 0},
            {Command.price_coins_to, 0},
            {Command.price_gems_from, 0},
            {Command.price_gems_to, 0},
            {Command.sort, SortingTypes.None},
        };

        /* Сообщения в чате пользователя */
        public readonly List<int> Messages = new();

        /* Прибыль, которую может получить пользователь, подсчитывается во время команды профиля */
        public int IncomeCoins;
        public int IncomeGems;
        private DateTime LastPayout;

        public async Task ClearMessages()
        {
            foreach (var messageId in Messages)
                await MessageController.DeleteMessage(user, messageId, false);
            Messages.Clear();
        }

        public async Task CalculateIncome()
        {
            IncomeCoins = 0;
            IncomeGems = 0;
            LastPayout = DateTime.Now;
            foreach (var sticker in user.Stickers.Values)
            {
                var stickerInfo = await StickerDao.GetStickerByHash(sticker.ShortHash);
                var payoutInterval = LastPayout - sticker.Payout;
                var payoutsCount = (int) (payoutInterval.TotalMinutes / stickerInfo.IncomeTime);
                if (payoutsCount < 1) continue;
                var multiplier = payoutsCount * sticker.Count;
                IncomeCoins += stickerInfo.IncomeCoins * multiplier;
                IncomeGems += stickerInfo.IncomeGems * multiplier;
            }
        }

        public async Task PayOut()
        {
            IncomeCoins = 0;
            IncomeGems = 0;
            foreach (var sticker in user.Stickers.Values)
            {
                var stickerInfo = await StickerDao.GetStickerByHash(sticker.ShortHash);
                var payoutInterval = LastPayout - sticker.Payout;
                var payoutsCount = (int) (payoutInterval.TotalMinutes / stickerInfo.IncomeTime);
                if (payoutsCount < 1) continue;
                var multiplier = payoutsCount * sticker.Count;
                sticker.Payout += new TimeSpan(0, stickerInfo.IncomeTime, 0) * payoutsCount;
                IncomeCoins += stickerInfo.IncomeCoins * multiplier;
                IncomeGems += stickerInfo.IncomeGems * multiplier;
            }
            user.Cash.Coins += IncomeCoins;
            user.Cash.Gems += IncomeGems;
        }

        public async Task PayOutOne(string hash)
        {
            IncomeCoins = 0;
            IncomeGems = 0;
            var sticker = user.Stickers[hash];
            var stickerInfo = await StickerDao.GetStickerByHash(hash);
            var payoutInterval = DateTime.Now - sticker.Payout;
            var payoutsCount = (int) (payoutInterval.TotalMinutes / stickerInfo.IncomeTime);
            if (payoutsCount < 1) return;
            var multiplier = payoutsCount * sticker.Count;
            sticker.Payout += new TimeSpan(0, stickerInfo.IncomeTime, 0) * payoutsCount;
            IncomeCoins += stickerInfo.IncomeCoins * multiplier;
            IncomeGems += stickerInfo.IncomeGems * multiplier;
            user.Cash.Coins += IncomeCoins;
            user.Cash.Gems += IncomeGems;
        }

        public async void EndSession()
        {
            await ClearMessages();
            
        }
        
        public void Dispose()
        {
            EndSession();
            GC.SuppressFinalize(this);
        }
    }
}