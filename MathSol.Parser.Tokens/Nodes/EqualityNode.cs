using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class EqualityNode(IAstNode left, IAstNode right) : BaseNode, IOperatorAstNode
{
    public string Operator => "=";

    public IAstNode Left => left;
    public IAstNode Right => right;

    public IEnumerable<IAstNode> Operands => [Left, Right];

    public override string ToString()
    {
        return $"{Left} = {Right}";
    }
}
