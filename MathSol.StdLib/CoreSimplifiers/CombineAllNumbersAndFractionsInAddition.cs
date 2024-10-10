using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class CombineAllNumbersAndFractionsInAddition : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || additionNode.Operands.Count(op => op is NumberNode) <= 1)
        {
            return node;
        }

        var numberNodesSum = additionNode.Operands.OfType<NumberNode>().Sum(number => number.Value);
        var leftOverOperands = additionNode.Operands.Where(operand => operand is not NumberNode);

        return new AdditionNode([.. leftOverOperands, new NumberNode(numberNodesSum)]);
    }
}
