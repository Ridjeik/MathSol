using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class StringNode(string @string) : BaseNode
{
    public string String => @string;

    public override string ToString()
    {
        return $"\"{@string}\"";
    }

    public override bool Equals(IAstNode? other)
    {
        return other is StringNode stringNode && stringNode.String == String;
    }
}
