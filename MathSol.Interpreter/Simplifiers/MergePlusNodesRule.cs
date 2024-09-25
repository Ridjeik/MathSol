using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Simplifiers;

internal class MergePlusNodesRule : INodeSimplificationRule
{
    public IAstNode Simplify(IAstNode node)
    {
        if (node is not PlusNode plusNode)
        {
            return node;
        }

        var newOperands = plusNode.Operands.SelectMany(o => o is PlusNode pn ? pn.Operands : [o]);
        return new PlusNode(newOperands.ToArray());
    }
}
