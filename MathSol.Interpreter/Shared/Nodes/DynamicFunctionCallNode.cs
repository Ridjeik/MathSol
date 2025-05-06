using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class DynamicFunctionCallNode(VariableNode potentiallyFunctionNode, IEnumerable<IAstNode> @params) : BaseNode, IOperatorAstNode
{
    public string Operator => potentiallyFunctionNode.Name;

    public IEnumerable<IAstNode> Operands => @params;

    public override string ToString()
    {
        return $"{potentiallyFunctionNode.Name}({string.Join(", ", @params)})";
    }
}
