using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class CombineAllNumbersAndFractionsInProducts : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode multiplicationNode || multiplicationNode.Operands.Count(op => op is NumberNode) <= 1)
        {
            return node;
        }

        var numberNodesProduct = multiplicationNode.Operands.OfType<NumberNode>().Select(n => n.Value).Aggregate((a, b) => a * b);
        var leftOverOperands = multiplicationNode.Operands.Where(operand => operand is not NumberNode);

        return new MultiplicationNode([new NumberNode(numberNodesProduct), .. leftOverOperands]);
    }
}
