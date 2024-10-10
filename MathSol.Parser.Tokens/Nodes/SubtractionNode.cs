using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class SubtractionNode(IAstNode left, IAstNode right) : BaseNode, IAstNodeWithOperands
{
    public IAstNode Left { get; private set; } = left;
    public IAstNode Right { get; private set; } = right;

    public IEnumerable<IAstNode> Operands => [Left, Right];

    public override string ToString()
    {
        return Left.ToString() + " - " + Right.ToString();
    }
}
