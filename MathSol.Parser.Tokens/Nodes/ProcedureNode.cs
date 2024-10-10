using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class ProcedureNode(string procedureName, params IAstNode[] operands) : BaseNode, IAstNodeWithOperands
{
    public string ProcedureName { get; } = procedureName;
    public IEnumerable<IAstNode> Operands { get; set; } = operands;

    public override string ToString()
    {
        return $"{ProcedureName} ({string.Join(", ", Operands)})";
    }

    public override bool Equals(IAstNode? other)
    {
        return base.Equals(other) && other is ProcedureNode node && node.ProcedureName == ProcedureName;
    }
}
