using System.Resources;
using CardCollector.Resources.Enums;

namespace CardCollector.Resources.Translations.Providers;

public class TopByTextsProvider : ResourceManagerProviderBase<TopBy, string>
{
    private static TopByTextsProvider? _instance;

    public static TopByTextsProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new TopByTextsProvider();
            return _instance;
        }
    }

    private TopByTextsProvider()
    {
    }
    
    protected override ResourceManager GetManager()
    {
        return TopByTexts.ResourceManager;
    }

    protected override string KeyToString(TopBy key)
    {
        return ((int) key).ToString();
    }

    protected override string? StringToValue(string? value)
    {
        return value;
    }
}