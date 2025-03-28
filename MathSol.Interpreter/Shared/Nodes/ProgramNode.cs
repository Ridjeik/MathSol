using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class ProgramNode(IEnumerable<IAstNode> statements) : BaseNode
{
    public IEnumerable<IAstNode> Statements { get; } = statements;

    public override string ToString() => string.Join(Environment.NewLine, Statements);
}
