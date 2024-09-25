using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Simplifiers;

internal class RemovePlusWithOneOperandRule : INodeSimplificationRule
{
    public IAstNode Simplify(IAstNode node)
    {
        if (node is PlusNode plusNode and { Operands.Count : 1 } ) 
        {
            return plusNode.Operands.Single();
        }

        return node;
    }
}
