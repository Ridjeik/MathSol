using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Utils;
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

    public FractionNode AsFraction()
    {
        if (IsInteger)
        {
            return new FractionNode((long)Value, 1);
        }

        var numerator = Value;
        var denominator = 1;
        while (numerator % 1 != 0)
        {             
            numerator *= 10;
            denominator *= 10;
        }

        var gcdValue = MathUtils.Gcd((long)numerator, denominator);

        return new FractionNode((long)numerator / gcdValue, denominator / gcdValue);
    }

    public override bool Equals(IAstNode? other)
    {
        return other is NumberNode numberNode && Value == numberNode.Value;
    }
}
