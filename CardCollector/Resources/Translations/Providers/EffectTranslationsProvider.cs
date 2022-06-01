using System.Resources;
using CardCollector.Resources.Enums;

namespace CardCollector.Resources.Translations.Providers;

public class EffectTranslationsProvider : ResourceManagerProviderBase<Effect, string>
{
    private static EffectTranslationsProvider? _instance;

    public static EffectTranslationsProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new EffectTranslationsProvider();
            return _instance;
        }
    }

    private EffectTranslationsProvider()
    {
    }

    protected override ResourceManager GetManager()
    {
        return EffectTranslations.ResourceManager;
    }

    protected override string KeyToString(Effect key)
    {
        return ((int) key).ToString();
    }

    protected override string? StringToValue(string? value)
    {
        return value;
    }
}