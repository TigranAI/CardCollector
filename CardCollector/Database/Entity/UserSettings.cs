using System.Collections.Generic;

namespace CardCollector.DataBase.Entity
{
    public class UserSettings
    {
        public UserSettings()
        {
            Settings = new Dictionary<UserSettingsEnum, bool>();
            Settings.Add(UserSettingsEnum.DailyTasks, true);
            Settings.Add(UserSettingsEnum.ExpGain, true);
            Settings.Add(UserSettingsEnum.StickerEffects, true);
            Settings.Add(UserSettingsEnum.DailyTaskProgress, true);
            Settings.Add(UserSettingsEnum.PiggyBankCapacity, true);
            Settings.Add(UserSettingsEnum.DailyExpTop, true);
        }
        
        public Dictionary<UserSettingsEnum, bool> Settings { get; set; }

        public void SwitchProperty(UserSettingsEnum property)
        {
            if (!Settings.ContainsKey(property))
                Settings.Add(property, true);
            Settings[property] = !Settings[property];
        }

        public bool this[UserSettingsEnum key]
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