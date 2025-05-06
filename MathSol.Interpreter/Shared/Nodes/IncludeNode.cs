namespace MathSol.Interpreter.Shared.Nodes;

public class IncludeNode(string fileName) : BaseNode
{
    public string FileName { get; set; } = fileName;

    public override string ToString()
    {
        return $"%include {this.FileName}";
    }
}
