using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class RemoveDivisionOfNonNumbers : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is DivisionNode { Dividend: NumberNode numerator, Divisor: NumberNode denominator })
        {
            return new FractionNode(numerator, denominator);
        }

        if (node is DivisionNode divisionNode)
        {
            return new MultiplicationNode(divisionNode.Dividend, new ExponentNode(divisionNode.Divisor, new NumberNode(-1)));
        }

        return node;
    }
}
