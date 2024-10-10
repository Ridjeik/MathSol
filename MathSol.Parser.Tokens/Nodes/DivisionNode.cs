using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class DivisionNode(IAstNode dividend, IAstNode divisor) : BaseNode, IOperatorAstNode
{
    public IAstNode Dividend { get; private set; } = dividend;
    public IAstNode Divisor { get; private set; } = divisor;

    public IEnumerable<IAstNode> Operands => [Dividend, Divisor];

    public string Operator => "/";

    public override string ToString()
    {
        return Dividend.ToString() + " / " + Divisor.ToString();
    }
}
