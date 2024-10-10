using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.BaseRules;
using MathSol.Interpreter.StdLib.Enums;

namespace MathSol.Interpreter.StdLib.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
internal class RemoveMultiplicationWithOneOperand : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not MultiplicationNode multiplicationNode || multiplicationNode.Operands.Count() > 1)
        {
            return node;
        }

        return multiplicationNode.Operands.Single();
    }
}
