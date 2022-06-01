using System.Resources;

namespace CardCollector.Resources.Translations.Providers;

internal class SortingTypesProvider : ResourceManagerProviderBase<Enums.SortingTypes, string>
{

    private static SortingTypesProvider? _instance;

    public static SortingTypesProvider Instance
    {
        get
        {
            if (_instance == null) _instance = new SortingTypesProvider();
            return _instance;
        }
    }

    private SortingTypesProvider()
    {
    }

    protected override ResourceManager GetManager()
    {
        return SortingTypes.ResourceManager;
    }

    protected override string KeyToString(Enums.SortingTypes key)
    {
        return ((int) key).ToString();
    }

    protected override string? StringToValue(string? value)
    {
        return value;
    }
}