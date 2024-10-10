using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.BaseRules;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class ConvertDifferenceIntoSum : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not SubtractionNode subtractionNode)
        {
            return node;
        }

        return new AdditionNode(subtractionNode.Left, new MultiplicationNode(new NumberNode(-1), subtractionNode.Right));
    }
}
