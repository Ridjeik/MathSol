﻿using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.BaseRules;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
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
