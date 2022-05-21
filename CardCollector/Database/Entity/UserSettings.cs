using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Resources.Enums;

namespace CardCollector.Database.Entity;

[Table("user_settings")]
public class UserSettings
{
    [Key, ForeignKey("id")]
    public virtual User User { get; set; }
    public Dictionary<UserSettingsTypes, bool> Settings { get; set; }
        
    public UserSettings()
    {
        Settings = new Dictionary<UserSettingsTypes, bool>();
        Settings.Add(UserSettingsTypes.DailyTasks, true);
        Settings.Add(UserSettingsTypes.ExpGain, true);
        Settings.Add(UserSettingsTypes.StickerEffects, true);
        Settings.Add(UserSettingsTypes.DailyTaskProgress, true);
        Settings.Add(UserSettingsTypes.PiggyBankCapacity, true);
        Settings.Add(UserSettingsTypes.DailyExpTop, true);
        Settings.Add(UserSettingsTypes.Distributions, true);
    }

    public void SwitchProperty(UserSettingsTypes property)
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