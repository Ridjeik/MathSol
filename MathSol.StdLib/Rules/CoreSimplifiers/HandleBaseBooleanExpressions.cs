using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class HandleBaseBooleanExpressions([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if(node is EqualityNode equalityNode && equalityNode.Left.Equals(equalityNode.Right))
        {
            return new BooleanNode(true);
        }

        return node;
    }
}
