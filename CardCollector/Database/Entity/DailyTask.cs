﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.UserDailyTask;

namespace CardCollector.DataBase.Entity
{
    public class DailyTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public User User { get; set; }
        public TaskKeys TaskId { get; set; }
        public int Progress { get; set; }
    }
}