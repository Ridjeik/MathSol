using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Utils;

namespace MathSol.Interpreter.Shared.Nodes;

public class FractionNode : BaseNode, IOperatorAstNode
{
    public long Numerator { get; private set; }
    public long Denominator { get; private set; }

    public IEnumerable<IAstNode> Operands => [new NumberNode(Numerator), new NumberNode(Denominator)];

    public string Operator => "/";

    public FractionNode(long numertator, long denominator)
    {
        if (denominator == 0)
            throw new ArgumentException("Denominator cannot be zero.");

        Numerator = numertator;
        Denominator = denominator;
    }

    public override string ToString()
    {
        return $"({Numerator} / {Denominator})";
    }

    public static FractionNode operator+ (FractionNode a, FractionNode b)
    {
        return Simplified(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    }

    public static FractionNode operator- (FractionNode a, FractionNode b)
    {
        return Simplified(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
    }

    public static FractionNode operator* (FractionNode a, FractionNode b)
    {
        return Simplified(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static FractionNode operator/ (FractionNode a, FractionNode b)
    {
        return Simplified(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }

    private static FractionNode Simplified(long a, long b)
    {
        var gcd = MathUtils.Gcd(Math.Abs(a), Math.Abs(b));
        return b > 0 ? new FractionNode(a / gcd, b / gcd) : new FractionNode(-a / gcd, -b / gcd);
    }
}
