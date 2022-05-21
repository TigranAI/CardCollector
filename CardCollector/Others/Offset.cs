using Telegram.Bot.Types;

namespace CardCollector.Others;

public struct Offset
{
    public int Value { get; }

    public Offset()
    {
        Value = -1;
    }
    
    public Offset(int val)
    {
        Value = val;
    }
    
    public static Offset Of(InlineQuery query)
    {
        if (query.Offset == "") return new Offset();
        return new Offset(int.Parse(query.Offset));
    }
    
    

    public Offset GetNext(int maxVal, int step = 50)
    {
        if (maxVal != -1 && Value + step >= maxVal) return new Offset();
        return this + step;
    }

    public override string ToString()
    {
        if (Value == -1) return "";
        return Value.ToString();
    }
    
    public static Offset operator+(Offset source, int val)
    {
        return new Offset(source.Value + val);
    }
}