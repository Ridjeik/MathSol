namespace MathSol.Interpreter.Shared.Nodes.Interfaces;

public interface IIdentifierAstNode : IAstNode
{
    public string Name { get; }
}
