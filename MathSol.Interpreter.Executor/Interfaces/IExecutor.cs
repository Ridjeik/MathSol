using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Executor.Interfaces;

public interface IExecutor
{
    public void Execute(IAstNode program);
}
