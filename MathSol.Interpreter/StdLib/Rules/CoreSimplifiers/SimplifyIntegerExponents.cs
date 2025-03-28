using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class SimplifyIntegerExponents([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not ExponentNode exponentNode || exponentNode.Power is not NumberNode { IsInteger: true, Value: var power})
        {
            return node;
        }

        if (exponentNode.Base is NumberNode @base)
        {
            var result = @base.Value;
            for (var i = 1; i < Math.Abs((long)power); i++)
            {
                result *= @base.Value;
            }

            var resultNode = new NumberNode(result);

            return power < 0 ? new DivisionNode(new NumberNode(1), resultNode) : resultNode;
        }

        if (exponentNode.Base is MultiplicationNode multiplicationNode)
        {
            var operands = multiplicationNode.Operands.Select(operand => new ExponentNode(operand, new NumberNode(power)));
            return new MultiplicationNode(operands.ToArray());
        }

        if (exponentNode.Base is ExponentNode baseExponentNode)
        {
            return new ExponentNode(baseExponentNode.Base, new MultiplicationNode(baseExponentNode.Power, new NumberNode(power)));
        }

        if (exponentNode.Base is FractionNode fractionNode)
        {
            return new DivisionNode(
                new ExponentNode(new NumberNode(fractionNode.Numerator), new NumberNode(power)),
                new ExponentNode(new NumberNode(fractionNode.Denominator), new NumberNode(power)));
        }

        return node;
    }
}
