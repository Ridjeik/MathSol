using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.BaseRules;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Rules.CoreSimplifiers;

[RuleType(Enums.RuleType.CoreSimplification)]
internal class RemoveUnaryMinuses : RecursiveRule
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not UnaryMinusNode unaryMinusNode)
        {
            return node;
        }

        return new MultiplicationNode(new NumberNode(-1), unaryMinusNode.Operand);
    }
}
