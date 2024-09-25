using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Simplifiers;

internal class UnaryPlusSimplifier : INodeSimplificationRule
{
    public IAstNode Simplify(IAstNode node)
    {
        if (node is UnaryPlusNode unaryPlusNode) return unaryPlusNode.Operand;
        return node;
    }
}
