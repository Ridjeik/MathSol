using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.BaseRules;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class BringNumbersAsLastOperandsInAddition : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || !additionNode.Operands.Any(op => op is NumberNode))
        {
            return node;
        }

        var numberNodes = additionNode.Operands.OfType<NumberNode>();
        var leftOverOperands = additionNode.Operands.Where(operand => operand is not NumberNode);

        return new AdditionNode([.. leftOverOperands, .. numberNodes]);
    }
}
