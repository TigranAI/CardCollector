using System.Resources;
using CardCollector.Database.Entity;
using CardCollector.UserDailyTask;

namespace CardCollector.Resources.Translations.Providers;

public class TitlesProvider : ResourceManagerProviderBase<DailyTask, string>
{
    private static TitlesProvider? _instance;

    public static TitlesProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new TitlesProvider();
            return _instance;
        }
    }

    private TitlesProvider()
    {
    }

    protected override ResourceManager GetManager()
    {
        return Titles.ResourceManager;
    }

    protected override string KeyToString(DailyTask key)
    {
        return key.TaskId.ToString();
    }

    protected override string? StringToValue(string? value)
    {
        return value;
    }
}