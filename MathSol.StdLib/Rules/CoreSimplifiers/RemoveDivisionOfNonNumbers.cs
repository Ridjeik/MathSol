using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
[RulePriority(1)]
internal class RemoveDivisionOfNonNumbers([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is DivisionNode { Dividend: NumberNode numerator, Divisor: NumberNode denominator } && numerator.IsInteger && denominator.IsInteger)
        {
            return new FractionNode((long)numerator.Value, (long)denominator.Value);
        }

        if (node is DivisionNode divisionNode)
        {
            return new MultiplicationNode(divisionNode.Dividend, new ExponentNode(divisionNode.Divisor, new NumberNode(-1)));
        }

        return node;
    }
}
