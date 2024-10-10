using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class ExponentNode(IAstNode @base, IAstNode power) : BaseNode, IOperatorAstNode
{
    public IAstNode Base { get; } = @base;
    public IAstNode Power { get; } = power;

    public IEnumerable<IAstNode> Operands => [Base, Power];

    public string Operator => "^";

    public override string ToString()
    {
        return $"{Base} ^ {Power}";
    }
}
