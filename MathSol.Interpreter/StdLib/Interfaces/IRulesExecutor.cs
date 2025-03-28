using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IRulesExecutor
{
    public IAstNode ExecuteRules(IAstNode node);
}
