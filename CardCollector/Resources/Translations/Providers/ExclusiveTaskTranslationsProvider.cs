using System.Resources;
using CardCollector.Resources.Enums;

namespace CardCollector.Resources.Translations.Providers;

public class ExclusiveTaskTranslationsProvider : ResourceManagerProviderBase<ExclusiveTask, string>
{
    private static ExclusiveTaskTranslationsProvider? _instance;

    public static ExclusiveTaskTranslationsProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new ExclusiveTaskTranslationsProvider();
            return _instance;
        }
    }

    private ExclusiveTaskTranslationsProvider()
    {
    }
    
    protected override ResourceManager GetManager()
    {
        return ExclusiveTaskTranslations.ResourceManager;
    }

    protected override string KeyToString(ExclusiveTask key)
    {
        return ((int) key).ToString();
    }

    protected override string? StringToValue(string? value)
    {
        return value;
    }
}