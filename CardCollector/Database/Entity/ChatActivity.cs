using System;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.DataBase.Entity
{
    [Owned]
    public class ChatActivity
    {
        public long MessageCount { get; set; }
        public DateTime LastGiveaway { get; set; } = DateTime.Now;
        public long MessageCountAtLastGiveaway { get; set; }
        public bool PrizeClaimed { get; set; }
    }
}