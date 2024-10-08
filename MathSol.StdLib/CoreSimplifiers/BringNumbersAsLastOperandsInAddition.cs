﻿using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class BringNumbersAsLastOperandsInAddition : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not AdditionNode additionNode || !additionNode.Operands.Any(op => op is NumberNode))
        {
            return node;
        }

        var numberNodes = additionNode.Operands.OfType<NumberNode>();
        var leftOverOperands = additionNode.Operands.Where(operand => operand is not NumberNode);

        return new AdditionNode([.. leftOverOperands, .. numberNodes]);
    }
}
