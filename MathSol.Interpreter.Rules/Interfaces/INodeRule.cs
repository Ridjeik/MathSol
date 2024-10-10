using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.Interfaces;

public interface INodeRule
{
    public IAstNode Execute(IAstNode node);
}
