using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class CombineAllNumbersAndFractionsInAddition([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || additionNode.Operands.Count(op => op is NumberNode or FractionNode) <= 1)
        {
            return node;
        }

        IEnumerable<IAstNode> operands = additionNode.Operands.ToList();

        decimal numberNodesSum = 0;
        if (operands.Any(op => op is NumberNode))
        {
            numberNodesSum = operands.OfType<NumberNode>().Sum(number => number.Value);
        }
        
        FractionNode fractionNodesSum = new(0, 1);
        if (operands.Any(op => op is FractionNode))
        {
            fractionNodesSum = operands.OfType<FractionNode>().Aggregate((a, b) => a + b);
        }

        var leftOverOperands = operands.Where(operand => operand is not (NumberNode or FractionNode));

        return new AdditionNode([.. leftOverOperands, fractionNodesSum + new NumberNode(numberNodesSum).AsFraction()]);
    }
}
