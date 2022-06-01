using System.Resources;

namespace CardCollector.Resources.Translations.Providers;

public abstract class ResourceManagerProviderBase<TK, TV>
{
    protected abstract ResourceManager GetManager();
    protected abstract string KeyToString(TK key);
    protected abstract TV? StringToValue(string? value);
    private TV? GetValue(TK key)
    {
        return StringToValue(GetManager().GetString(KeyToString(key)));
    }

    public TV? this[TK key] => GetValue(key);
}