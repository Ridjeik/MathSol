using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Parser.AstNodes;

internal class UnaryPlusNode(IAstNode operand) : IAstNode
{
    public IAstNode Operand { get; } = operand;

    public bool Equals(IAstNode? other)
    {
        return other is UnaryPlusNode unaryPlusNode && unaryPlusNode.Operand.Equals(this.Operand);
    }

    public override string ToString()
    {
        return '+' + this.Operand.ToString();
    }
}
