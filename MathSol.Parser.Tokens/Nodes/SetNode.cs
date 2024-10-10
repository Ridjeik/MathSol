using MathSol.Interpreter.Shared.Nodes.Interfaces;
using System.Collections;

namespace MathSol.Interpreter.Shared.Nodes;

public class SetNode(IEnumerable<IAstNode> operands) : BaseNode, IEnumerable<IAstNode>
{
    public IEnumerable<IAstNode> Operands { get; } = operands;

    public IEnumerator<IAstNode> GetEnumerator()
    {
        return Operands.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Operands.GetEnumerator();
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", Operands)}]";
    }
}
