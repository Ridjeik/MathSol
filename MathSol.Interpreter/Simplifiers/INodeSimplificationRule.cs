using MathSol.Interpreter.Parser.Interfaces;

namespace MathSol.Interpreter.Simplifiers;

internal interface INodeSimplificationRule
{
    public IAstNode Simplify(IAstNode node);
}
