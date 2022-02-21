using System;
using System.Collections.Generic;
using System.Linq;
using CardCollector.DataBase.Entity;
using CardCollector.Others;
using CardCollector.Resources;

namespace CardCollector.Session.Modules
{
    public class CombineModule : Module
    {
        public long? SelectedStickerId;
        public int Count = 1;

        public List<Tuple<Sticker, int>> CombineList { get; } = new();

        public int CombineCount => CombineList.Sum(item => item.Item2);
        public int? CombineTier => CombineList.FirstOrDefault()?.Item1.Tier;
        
        public int? CombinePrice
        {
            get
            {
                return CombineList.FirstOrDefault()?.Item1.Tier switch
                {
                    1 => 200,
                    2 => 500,
                    3 => 1200,
                    _ => null
                };
            }
        }

        public void Reset()
        {
            SelectedStickerId = null;
            Count = 1;
            CombineList.Clear();
        }

        public void AddSticker(Sticker sticker, int count)
        {
            CombineList.Add(new Tuple<Sticker, int>(sticker, count));
        }
        
        public override string ToString()
        {
            var message = $"{Text.added_stickers} {CombineCount}/{Constants.COMBINE_COUNT}:";
            foreach (var ((sticker, count), index) in CombineList.WithIndex())
                message += $"\n{Text.sticker} {index + 1}: {sticker.Title} {count}{Text.items}";
            return message;
        }
    }
}