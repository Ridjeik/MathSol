using System.Globalization;

namespace MathSol.Interpreter.Shared.Nodes;

public class NumberNode(decimal value) : BaseNode
{
    public decimal Value { get; } = value;

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public bool IsInteger => Value % 1 == 0;
}
