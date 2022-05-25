using Telegram.Bot.Types;

namespace CardCollector.Others;

public struct Offset
{
    public readonly int Value { get; }
    public readonly int Step { get; }

    public Offset()
    {
        Value = 0;
        Step = 50;
    }
    
    public Offset(int val)
    {
        Value = val;
        Step = 50;
    }

    public Offset(int value, int step)
    {
        Value = value;
        Step = step;
    }

    public static Offset Of(InlineQuery query)
    {
        if (query.Offset == "") return new Offset();
        return new Offset(int.Parse(query.Offset));
    }
    
    

    public Offset GetNext(int maxVal, int step = 50)
    {
        if (Value + step >= maxVal) return new Offset();
        return new Offset(Value + step, step);
    }

    public override string ToString()
    {
        if (Value == 0) return "";
        return Value.ToString();
    }
}