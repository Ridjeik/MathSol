using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class CharNode(char @char) : BaseNode
{
    public char Char => @char;

    public override bool Equals(IAstNode? other)
    {
        return other is CharNode node && node.Char == Char;
    }

    public override string ToString()
    {
        return Char.ToString();
    }
}
