using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class FunctionDeclarationNode(FunctionNode function, IAstNode body) : BaseNode
{
    public FunctionNode Function { get; } = function;
    public IAstNode Body { get; set; } = body;

    public override string ToString()
    {
        return $"{Function} = {Body}";
    }

    public FunctionDeclarationNode WithBody(IAstNode body)
    {
        return new FunctionDeclarationNode(Function, body);
    }
}
