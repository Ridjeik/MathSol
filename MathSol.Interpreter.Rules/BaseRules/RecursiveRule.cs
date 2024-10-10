using MathSol.Interpreter.Rules.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.BaseRules;

internal abstract class RecursiveRule : INodeRule
{
    public IAstNode Execute(IAstNode node)
    {


        return ExecuteRecursive(node);
    }

    protected abstract IAstNode ExecuteRecursive(IAstNode node);
}
