using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class IfNode(IAstNode condition, IAstNode ifBody, IAstNode? elseBody = null) : IAstNode
{
    public IAstNode Condition { get; } = condition;
    public IAstNode IfBody { get; } = ifBody;
    public IAstNode? ElseBody { get; set; } = elseBody;

    public override string ToString()
    {
        return $"if({Condition})\n{{\n{IfBody}\n}}{(ElseBody is not null ? $"\nelse\n{{\n{ElseBody}\n}}" : "")}";
    }

    public bool Equals(IAstNode? other) => other is IfNode node && Condition.Equals(node.Condition) && IfBody.Equals(node.IfBody);
}
