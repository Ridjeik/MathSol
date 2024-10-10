using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class MergeSubSumsIntoSum : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || !additionNode.Operands.OfType<AdditionNode>().Any())
        {
            return node;
        }

        var leftOverOperands = additionNode.Operands.Where(operand => operand is not AdditionNode);
        var subSums = additionNode.Operands.OfType<AdditionNode>().SelectMany(subSum => subSum.Operands);

        return new AdditionNode([.. leftOverOperands, .. subSums]);
    }
}
