using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class SetNode(IEnumerable<IAstNode> operands) : BaseNode, IAstNodeWithOperands
{
    public IEnumerable<IAstNode> Operands { get; } = operands;

    public override string ToString()
    {
        return $"[{string.Join(", ", Operands)}]";
    }
}
