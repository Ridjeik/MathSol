using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class FractionNode(NumberNode numertator, NumberNode denominator) : BaseNode, IAstNodeWithOperands
{
    public NumberNode Numerator { get; private set; } = numertator;
    public NumberNode Denominator { get; private set; } = denominator;

    public IEnumerable<IAstNode> Operands => [Numerator, Denominator];

    public override string ToString()
    {
        return $"{Numerator} / {Denominator}";
    }
}
