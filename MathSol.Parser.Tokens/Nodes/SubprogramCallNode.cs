using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class SubprogramCallNode(string procedureName, params IAstNode[] operands) : BaseNode, IOperatorAstNode, IIdentifierAstNode
{
    public string Name { get; } = procedureName;
    public IEnumerable<IAstNode> Operands { get; set; } = operands;

    public string Operator => Name;

    public override string ToString()
    {
        return $"{Name} ({string.Join(", ", Operands)})";
    }

    public override bool Equals(IAstNode? other)
    {
        return base.Equals(other) && other is SubprogramCallNode node && node.Name == Name;
    }
}
