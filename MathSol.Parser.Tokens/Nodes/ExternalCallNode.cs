using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class ExternalCallNode : IAstNode
{
    public bool Equals(IAstNode? other) => other is ExternalCallNode;

    public override string ToString() => "External Code";
}
