using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class ReturnNode(IAstNode value) : BaseNode
{
    public IAstNode ReturnValue { get; } = value;

    public override string ToString()
    {
        return $"return {ReturnValue}";
    }
}
