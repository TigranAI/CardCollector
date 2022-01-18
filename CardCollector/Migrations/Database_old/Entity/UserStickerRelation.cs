using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("user_to_stickers_relations")]
    public class UserStickerRelation
    {
        [Column("id"), MaxLength(127)] public long Id { get; set; }
        [Column("sticker_id"), MaxLength(127)] public string StickerId { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("count"), MaxLength(32)] public int Count { get; set; }
        [Column("payout"), MaxLength(32)] public DateTime Payout { get; set; }
        [Column("short_hash"), MaxLength(40)] public string ShortHash { get; set; }
        [Column("additional_data"), MaxLength(512)] public string? AdditionalData { get; set; }
    }
}