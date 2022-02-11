using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    public class SpecialOrderUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual User User { get; set; }
        public virtual SpecialOrder Order { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}