using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class UnaryMinusNode(IAstNode operand) : BaseNode, IAstNodeWithOperands
{
    public IAstNode Operand { get; } = operand;

    public IEnumerable<IAstNode> Operands { get; set; } = [operand];

    public override string ToString()
    {
        return '-' + Operand.ToString();
    }
}
