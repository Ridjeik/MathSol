using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class AdditionNode(params IAstNode[] operands) : BaseNode, IOperatorAstNode
{
    public IEnumerable<IAstNode> Operands { get; set; } = [.. operands];

    public string Operator => "+";

    public override string ToString()
    {
        return Operands.Aggregate(string.Empty, (current, operand) => $"{current}{(current != string.Empty ? " " : "")}{NextOperand(operand, current == string.Empty)}");
    }

    private string NextOperand(IAstNode operand, bool first)
    {
        if(operand is NumberNode { Value: < 0 } number)
        {
            return $"- {-number.Value}";
        }
        if (operand is UnaryMinusNode unary)
        {
            return $"-({unary.Operand})";
        }
        return first ? $"{operand}" : $"+ {operand}";
    }
}
