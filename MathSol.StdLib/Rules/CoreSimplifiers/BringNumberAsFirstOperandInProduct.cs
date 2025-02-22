using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class BringNumbersAsFirstOperandsInProduct([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode productNode || !productNode.Operands.Any(op => op is NumberNode or FractionNode))
        {
            return node;
        }

        var numberNodes = productNode.Operands.OfType<NumberNode>();
        var fractionNodes = productNode.Operands.OfType<FractionNode>();
        var leftOverOperands = productNode.Operands.Where(operand => operand is not (NumberNode or FractionNode));

        return new MultiplicationNode([.. fractionNodes, .. numberNodes, .. leftOverOperands]);
    }
}
