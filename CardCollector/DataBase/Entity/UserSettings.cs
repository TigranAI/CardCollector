using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CardCollector.DataBase.Entity
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

        [NotMapped] private Dictionary<UserSettingsEnum, bool> settings = new ();

        private bool GetProperty(UserSettingsEnum property)
        {
            if (!settings.ContainsKey(property))
                settings.Add(property, true);
            return settings[property];
        }

        private void SetProperty(UserSettingsEnum property, bool value)
        {
            if (!settings.ContainsKey(property))
                settings.Add(property, true);
            settings[property] = value;
        }

        public void SwitchProperty(UserSettingsEnum property)
        {
            if (!settings.ContainsKey(property))
                settings.Add(property, true);
            settings[property] = !settings[property];
        }

        public void InitProperties()
        {
            var props = Enum.GetValues<UserSettingsEnum>();
            foreach (var prop in props) settings.Add(prop, true);
        }

        public bool this[UserSettingsEnum key]
        {
            get => GetProperty(key);
            set => SetProperty(key, value);
        }
    }
    
    public enum UserSettingsEnum
    {
        DailyTasks,
        ExpGain,
        StickerEffects,
        DailyTaskProgress,
        PiggyBankCapacity,
        DailyExpTop,
    }
}