using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Simplifiers;

internal class MergeNumbersInAdditionRule : INodeSimplificationRule
{
    public IAstNode Simplify(IAstNode node)
    {
        if (node is not PlusNode plusNode || plusNode.Operands.Count(op => op is NumberNode) <= 1)
        {
            return node;
        }

        var otherOperands = plusNode.Operands.Where(op => op is not NumberNode);
        var newNumberOperand = new NumberNode(plusNode.Operands
            .Where(op => op is NumberNode)
            .Cast<NumberNode>()
            .Sum(n => n.Value));

        return new PlusNode([.. otherOperands, newNumberOperand]);
    }
}
