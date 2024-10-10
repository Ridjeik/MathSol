using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.Interfaces;

public interface IRulesExecutor
{
    public IAstNode ExecuteRules(IAstNode node);
}
