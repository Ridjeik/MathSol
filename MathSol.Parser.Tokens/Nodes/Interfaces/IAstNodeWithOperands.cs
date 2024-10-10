namespace MathSol.Interpreter.Shared.Nodes.Interfaces;

public interface IAstNodeWithOperands : IAstNode
{
    IEnumerable<IAstNode> Operands { get; }
}
