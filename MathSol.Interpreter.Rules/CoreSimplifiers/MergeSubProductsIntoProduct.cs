using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.BaseRules;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class MergeSubProductsIntoProduct : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode multiplicationNode || !multiplicationNode.Operands.OfType<MultiplicationNode>().Any())
        {
            return node;
        }

        var leftOverOperands = multiplicationNode.Operands.Where(operand => operand is not MultiplicationNode);
        var subProducts = multiplicationNode.Operands.OfType<MultiplicationNode>().SelectMany(subProduct => subProduct.Operands);

        return new MultiplicationNode([.. leftOverOperands, .. subProducts]);
    }
}
