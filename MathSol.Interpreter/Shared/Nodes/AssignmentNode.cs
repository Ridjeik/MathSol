using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class AssignmentNode(VariableNode left, IAstNode right) : BaseNode, IOperatorAstNode
{
    public string Operator => ":=";

    public IEnumerable<IAstNode> Operands => [Left, Right];

    public VariableNode Left { get; } = left;
    public IAstNode Right { get; } = right;

    public override string ToString() => $"{Left} {Operator} {Right}";
}
