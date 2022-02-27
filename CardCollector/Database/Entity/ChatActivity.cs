using System;
using Microsoft.EntityFrameworkCore;

namespace CardCollector.Database.Entity
{
    [Owned]
    public class ChatActivity
    {
        public long MessageCount { get; set; }
        public DateTime? LastGiveaway { get; set; }
        public long MessageCountAtLastGiveaway { get; set; }
        public bool PrizeClaimed { get; set; }
        public bool GiveawayAvailable { get; set; }
    }
}