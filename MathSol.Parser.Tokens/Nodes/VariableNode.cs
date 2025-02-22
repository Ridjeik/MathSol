using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class VariableNode(string name) : BaseNode, IIdentifierAstNode
{
    public string Name { get; } = name;

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(IAstNode? other)
    {
        return other is VariableNode variableNode && variableNode.Name == this.Name;
    }
}
