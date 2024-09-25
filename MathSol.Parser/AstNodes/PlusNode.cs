namespace MathSol.Parser.AstNodes;

internal class PlusNode(IAstNode leftOperand, IAstNode rightOperand) : IAstNode
{
    public IAstNode LeftOperand { get; } = leftOperand;
    public IAstNode RightOperand { get; } = rightOperand;
}
