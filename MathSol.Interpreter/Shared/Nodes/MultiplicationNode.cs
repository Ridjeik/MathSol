using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class MultiplicationNode(params IAstNode[] operands) : BaseNode, IOperatorAstNode
{
    public IEnumerable<IAstNode> Operands { get; set; } = operands.AsEnumerable();

    public string Operator => "*";

    public override string ToString()
    {
        if (Operands.Count() == 2)
        {
            if (Operands.First() is NumberNode number && Operands.ElementAt(1) is not VariableNode variable)
            {
                return $"{number.Value} * ({Operands.ElementAt(1)})";
            }

            return $"({Operands.First()}) * ({Operands.ElementAt(1)})";
        }

        return string.Join("*", Operands.Select(o => o.ToString()));
    }
}
