namespace MathSol.Interpreter.Shared.Nodes.Interfaces;

public interface IOperatorAstNode : IAstNode
{
    string Operator { get; }
    IEnumerable<IAstNode> Operands { get; }
}
