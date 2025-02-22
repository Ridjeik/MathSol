using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class FunctionCallNode(string function, IEnumerable<IAstNode> parameters) : BaseNode, IOperatorAstNode
{
    public string Operator => Function;

    public IEnumerable<IAstNode> Operands => Arguments;

    public string Function { get; } = function;
    public IEnumerable<IAstNode> Arguments { get; } = parameters;

    public override string ToString()
    {
        return $"{Function}({string.Join(", ", Arguments)})";
    }
}
