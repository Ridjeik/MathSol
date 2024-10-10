using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class MultiplicationNode(params IAstNode[] operands) : BaseNode, IOperatorAstNode
{
    public IEnumerable<IAstNode> Operands { get; set; } = [.. operands];

    public string Operator => "*";

    public override string ToString()
    {
        return string.Join(" * ", Operands.Select(o => o.ToString()));
    }
}
