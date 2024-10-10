using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;

namespace MathSol.Interpreter.StdLib.BaseRules;

internal abstract class RecursiveRule : INodeRule
{
    public IAstNode Execute(IAstNode node)
    {


        return ExecuteRecursive(node);
    }

    protected abstract IAstNode ExecuteRecursive(IAstNode node);
}
