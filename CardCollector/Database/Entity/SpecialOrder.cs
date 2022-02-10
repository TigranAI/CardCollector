using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    public class SpecialOrder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Pack Pack { get; set; }
        [MaxLength(256)] public string Title { get; set; }
        public bool IsInfinite { get; set; }
        public int Count { get; set; }
        public int PriceCoins { get; set; }
        public int PriceGems { get; set; }
        public int Discount { get; set; }
        public bool TimeLimited { get; set; }
        public DateTime? TimeLimit { get; set; }
        [MaxLength(256)] public string? AdditionalPrize { get; set; }
        [MaxLength(256)] public string? Description { get; set; }
        public string? PreviewFileId { get; set; }
        public bool IsPreviewAnimated { get; set; }

        public bool IsExpired()
        {
            if (!TimeLimited) return false;
            if (TimeLimit == null) return false;
            if (TimeLimit < DateTime.Now) return true;
            return false;
        }

        public int GetResultPriceCoins()
        {
            return PriceCoins - PriceCoins * Discount / 100;
        }

        public int GetResultPriceGems()
        {
            return PriceGems - PriceGems * Discount / 100;
        }
    }
}