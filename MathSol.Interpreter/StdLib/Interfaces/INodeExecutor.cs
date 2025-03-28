using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface INodeExecutor
{
    public (IAstNode result, bool isTerminated) Execute(IAstNode program);
}
