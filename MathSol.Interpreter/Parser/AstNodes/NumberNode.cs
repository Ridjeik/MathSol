using MathSol.Interpreter.Parser.Interfaces;
using System.Globalization;

namespace MathSol.Interpreter.Parser.AstNodes;

internal class NumberNode(decimal value) : IAstNode
{
    public decimal Value { get; } = value;

    public bool Equals(IAstNode? other)
    {
        return other is NumberNode numberNode && this.Value == numberNode.Value;
    }

    public override string ToString()
    {
        return this.Value.ToString(CultureInfo.InvariantCulture);
    }
}
