namespace MathSol.Parser.AstNodes;

public class ActionNode(string action, IAstNode operand) : IAstNode
{
    public string Action { get; } = action;
    public IAstNode Operand { get; } = operand;
}
