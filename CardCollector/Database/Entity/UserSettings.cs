using System.Collections.Generic;

namespace CardCollector.Database.Entity
{
    public class UserSettings
    {
        public Dictionary<Resources.Enums.UserSettings, bool> Settings { get; set; }
        
        public UserSettings()
        {
            Settings = new Dictionary<Resources.Enums.UserSettings, bool>();
            Settings.Add(Resources.Enums.UserSettings.DailyTasks, true);
            Settings.Add(Resources.Enums.UserSettings.ExpGain, true);
            Settings.Add(Resources.Enums.UserSettings.StickerEffects, true);
            Settings.Add(Resources.Enums.UserSettings.DailyTaskProgress, true);
            Settings.Add(Resources.Enums.UserSettings.PiggyBankCapacity, true);
            Settings.Add(Resources.Enums.UserSettings.DailyExpTop, true);
        }

        public void SwitchProperty(Resources.Enums.UserSettings property)
        {
            if (!Settings.ContainsKey(property))
                Settings.Add(property, true);
            Settings[property] = !Settings[property];
        }

        public bool this[Resources.Enums.UserSettings key]
        {
            get
            {
                if (!Settings.ContainsKey(key))
                    Settings.Add(key, true);
                return Settings[key];
            }
            set
            {
                if (!Settings.ContainsKey(key))
                    Settings.Add(key, value);
                else Settings[key] = value;
            }
        }
    }
}