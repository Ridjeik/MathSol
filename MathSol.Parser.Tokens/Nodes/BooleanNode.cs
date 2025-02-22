namespace MathSol.Interpreter.Shared.Nodes;

public class BooleanNode(bool value) : BaseNode
{
    public bool Value { get; } = value;

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator bool(BooleanNode node)
    {
        return node.Value;
    }

    public static implicit operator BooleanNode(bool value)
    {
        return new BooleanNode(value);
    }

    public static BooleanNode operator !(BooleanNode node)
    {
        return new BooleanNode(!node.Value);
    }

    public static BooleanNode operator &(BooleanNode left, BooleanNode right)
    {
        return new BooleanNode(left.Value & right.Value);
    }

    public static BooleanNode operator |(BooleanNode left, BooleanNode right)
    {
        return new BooleanNode(left.Value | right.Value);
    }

    public static BooleanNode operator ^(BooleanNode left, BooleanNode right)
    {
        return new BooleanNode(left.Value ^ right.Value);
    }
}
