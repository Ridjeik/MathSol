using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface INodeRule
{
    public IAstNode Execute(IAstNode node);
}
