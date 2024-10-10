namespace MathSol.Interpreter.Shared.Nodes;

public class StringNode(string @string) : BaseNode
{
    public string String => @string;

    public override string ToString()
    {
        return $"\"{@string}\"";
    }
}
