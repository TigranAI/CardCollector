using System.Collections.Generic;
using System.Linq;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;

namespace CardCollector.Session.Modules
{
    public class CombineModule : Module
    {
        public StickerEntity SelectedSticker;
        public int Count = 1;
        public int Tier => SelectedSticker.Tier;
        public Dictionary<StickerEntity, int> CombineList { get; } = new();
        
        public int CalculateCombinePrice()
        {
            var coinsSum = CombineList.Sum(pair => 1440 / pair.Key.IncomeTime * pair.Key.Income * pair.Value);
            var multiplier = SelectedSticker.Tier * 0.25 + 1;
            return (int)(coinsSum * multiplier);
        }
        
        public int GetCombineCount()
        {
            return CombineList.Values.Sum();
        }
        
        public void Reset()
        {
            SelectedSticker = null;
            Count = 0;
            CombineList.Clear();
        }
        
        public override string ToString()
        {
            var message = $"{Text.added_stickers} {GetCombineCount()}/{Constants.COMBINE_COUNT}:";
            foreach (var ((sticker, count), index) in CombineList.WithIndex())
                message += $"\n{Text.sticker} {index + 1}: {sticker.Title} {count}{Text.items}";
            return message;
        }
    }
}