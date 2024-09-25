namespace MathSol.Parser.AstNodes;

internal class NumberNode(decimal value) : IAstNode
{
    public decimal Value { get; } = value;
}
