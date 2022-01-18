using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CardCollector.DataBase.Entity;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("user_settings")]
    public class UserSettings
    {
        [Key, Column("id"), MaxLength(127)] public long UserId { get; set; }

        [Column("settings"), MaxLength(512)] public string Settings {
            get {
                return Utilities.ToJson(settings.ToDictionary(item => (int) item.Key, item => Convert.ToInt32(item.Value)));
            }
            set
            {
                var dict = Utilities.FromJson<Dictionary<int, bool>>(value);
                settings = dict.ToDictionary(item => (UserSettingsEnum) item.Key, item => Convert.ToBoolean(item.Value));
            }
        }

        [NotMapped] public Dictionary<UserSettingsEnum, bool> settings = new ();
    }
}