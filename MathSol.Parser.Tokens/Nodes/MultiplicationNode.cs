using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class MultiplicationNode(params IAstNode[] operands) : BaseNode, IAstNodeWithOperands
{
    public IEnumerable<IAstNode> Operands { get; set; } = [.. operands];

    public override string ToString()
    {
        return string.Join(" * ", Operands.Select(o => o.ToString()));
    }
}
