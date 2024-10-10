namespace MathSol.Interpreter.Shared.Nodes;

public class VariableNode(string name) : BaseNode
{
    public string Name { get; } = name;

    public override string ToString()
    {
        return Name;
    }
}
