using System.Collections.Generic;

namespace CardCollector.Database.Entity
{
    public class UserSettings
    {
        public Dictionary<Resources.Enums.UserSettingsTypes, bool> Settings { get; set; }
        
        public UserSettings()
        {
            Settings = new Dictionary<Resources.Enums.UserSettingsTypes, bool>();
            Settings.Add(Resources.Enums.UserSettingsTypes.DailyTasks, true);
            Settings.Add(Resources.Enums.UserSettingsTypes.ExpGain, true);
            Settings.Add(Resources.Enums.UserSettingsTypes.StickerEffects, true);
            Settings.Add(Resources.Enums.UserSettingsTypes.DailyTaskProgress, true);
            Settings.Add(Resources.Enums.UserSettingsTypes.PiggyBankCapacity, true);
            Settings.Add(Resources.Enums.UserSettingsTypes.DailyExpTop, true);
        }

        public void SwitchProperty(Resources.Enums.UserSettingsTypes property)
        {
            if (!Settings.ContainsKey(property))
                Settings.Add(property, true);
            Settings[property] = !Settings[property];
        }

        public bool this[Resources.Enums.UserSettingsTypes key]
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