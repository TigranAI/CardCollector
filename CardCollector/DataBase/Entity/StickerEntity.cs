using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    /* Объект таблицы stickers (одна строка)
     Здесь хранится Id стикера с серверов Telegram
     Наименование стикера, Автор стикера, Доход в монетах,
     Доход в алмазах, Количество звезд, Эмоции связанные со стикером,
     Описание стикера
     */
    [Table("stickers")]
    public class StickerEntity
    {
        /* Id стикера на сервере Телеграм */
        [Column("id"), MaxLength(127)] public string Id { get; set; }
        
        /* Название стикера */
        [Column("title"), MaxLength(256)] public string Title { get; set; }
        
        /* Автор стикера */
        [Column("author"), MaxLength(128)] public string Author { get; set; }
        
        /* Доход от стикера в монетах */
        [Column("income_coins"), MaxLength(32)] public int IncomeCoins { get; set; } = 0;
        
        /* Доход от стикера в алмазах */
        [Column("income_gems"), MaxLength(32)] public int IncomeGems { get; set; } = 0;
        
        /* Время, необходимое для получения дохода */
        [Column("income_time"), MaxLength(32)] public int IncomeTime { get; set; } = 0;
        
        /* Стоимость стикера в магазине */
        [Column("income_time"), MaxLength(32)] public int Price { get; set; } = 0;
        
        /* Количество звезд стикера (редкость) */
        [Column("tier"), MaxLength(32)] public int Tier { get; set; } = 1;
        
        /* Эмоции, связанные со стикером */
        [Column("emoji"), MaxLength(127)] public string Emoji { get; set; } = "";
        
        /* Описание стикера */
        [Column("description"), MaxLength(1024)] public string Description { get; set; } = "";
        
        /* Хеш id стикера для определения его в системе */
        [Column("md5hash"), MaxLength(40)] public string Md5Hash { get; set; }
    }
}