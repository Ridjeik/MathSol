using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
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
