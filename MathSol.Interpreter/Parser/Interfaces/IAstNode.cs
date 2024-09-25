using MathSol.Interpreter.Simplifiers;

namespace MathSol.Interpreter.Parser.Interfaces;

internal interface IAstNode : IEquatable<IAstNode>
{
    IAstNode SimplifyByRule(INodeSimplificationRule rule)
    {
        return rule.Simplify(this);
    }
}
