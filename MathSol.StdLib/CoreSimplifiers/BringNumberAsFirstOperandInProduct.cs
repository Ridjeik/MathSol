using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class BringNumbersAsFirstOperandsInProduct : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode productNode || !productNode.Operands.Any(op => op is NumberNode))
        {
            return node;
        }

        var numberNodes = productNode.Operands.OfType<NumberNode>();
        var leftOverOperands = productNode.Operands.Where(operand => operand is not NumberNode);

        return new MultiplicationNode([.. numberNodes, .. leftOverOperands]);
    }
}
