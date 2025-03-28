using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class BringNumbersAsLastOperandsInAddition([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || !additionNode.Operands.Any(op => op is NumberNode))
        {
            return node;
        }

        var numberNodes = additionNode.Operands.OfType<NumberNode>();
        var fractionNodes = additionNode.Operands.OfType<FractionNode>();
        var leftOverOperands = additionNode.Operands.Where(operand => operand is not (NumberNode or FractionNode));

        return new AdditionNode([.. leftOverOperands, .. numberNodes, ..fractionNodes]);
    }
}
