using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class SubprogramNode(string name, IEnumerable<VariableNode> @params) : IIdentifierAstNode
{
    public string Name { get; } = name;
    public IEnumerable<VariableNode> Params { get; } = @params;

    public bool Equals(IAstNode? other) => other is SubprogramNode node && Name == node.Name && Params.SequenceEqual(node.Params);

    public override string ToString() => $" {Name}({string.Join(", ", Params)})";
}
