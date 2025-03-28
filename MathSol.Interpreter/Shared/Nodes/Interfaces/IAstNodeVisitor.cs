namespace MathSol.Interpreter.Shared.Nodes.Interfaces;

public interface IAstNodeVisitor
{
    IAstNode Visit(IAstNode node);
}
