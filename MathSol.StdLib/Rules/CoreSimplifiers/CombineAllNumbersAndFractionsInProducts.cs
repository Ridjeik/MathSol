using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class CombineAllNumbersAndFractionsInProducts([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode multiplicationNode || multiplicationNode.Operands.Count(op => op is (NumberNode or FractionNode)) <= 1)
        {
            return node;
        }

        decimal numberNodesProduct = 1;
        if (multiplicationNode.Operands.Any(op => op is NumberNode))
        {
            numberNodesProduct = multiplicationNode.Operands.OfType<NumberNode>().Select(n => n.Value).Aggregate((a, b) => a * b);
        }
        FractionNode fractionNodesProduct = new FractionNode(1, 1);
        if (multiplicationNode.Operands.Any(op => op is FractionNode))
        {
            fractionNodesProduct = multiplicationNode.Operands.OfType<FractionNode>().Aggregate((a, b) => a * b);
        }
        var leftOverOperands = multiplicationNode.Operands.Where(operand => operand is not (NumberNode or FractionNode));

        return new MultiplicationNode([new NumberNode(numberNodesProduct).AsFraction() * fractionNodesProduct, .. leftOverOperands]);
    }
}
