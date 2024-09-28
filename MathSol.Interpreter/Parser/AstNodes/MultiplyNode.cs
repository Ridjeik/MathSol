using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Simplifiers;

namespace MathSol.Interpreter.Parser.AstNodes;

internal class MultiplyNode(params IAstNode[] operands) : IAstNode
{
    public List<IAstNode> Operands { get; } = [.. operands];

    public IAstNode SimplifyByRule(INodeSimplificationRule rule)
    {
        var newOperands = Operands.Select(op => op.SimplifyByRule(rule));

        return rule.Simplify(new MultiplyNode([.. newOperands]));
    }

    public bool Equals(IAstNode? other)
    {
        return other is MultiplyNode plusNode && plusNode.Operands.SequenceEqual(this.Operands);
    }

    public override string ToString()
    {
        return string.Join(" * ", this.Operands.Select(o => o.ToString()));
    }
}
