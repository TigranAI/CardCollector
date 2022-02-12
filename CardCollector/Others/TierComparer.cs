﻿using System.Collections.Generic;
using CardCollector.DataBase.Entity;

namespace CardCollector.Others
{
    public class TierComparer : IComparer<Sticker>
    {
        public int Compare(Sticker? x, Sticker? y)
        {
            return x?.Tier - y?.Tier ?? 0;
        }
    }
}