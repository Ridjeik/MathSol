using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Simplifiers;

namespace MathSol.Interpreter.Parser.AstNodes;

internal class ActionNode(string action, IAstNode operand) : IAstNode
{
    public string Action { get; } = action;
    public IAstNode Operand { get; } = operand;

    public bool Equals(IAstNode? other)
    {
        return other is ActionNode actionNode && actionNode.Action == this.Action && actionNode.Operand.Equals(this.Operand);
    }

    public override string ToString()
    {
        return $"{Action} ({Operand})";
    }
}
