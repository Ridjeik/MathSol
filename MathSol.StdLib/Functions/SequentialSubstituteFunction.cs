using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("sequential_substitute")]
[FunctionParametersCount(2)]
internal class SequentialSubstituteFunction([FromKeyedServices("substitute")] IBuiltinFunctionImplementation substitute) : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression", "substitutions"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        if (astNodes[1] is not SetNode set || set.Operands.Any(operand => operand is not EqualityNode))
        {
            throw new ArgumentException("Second argument must be a set of equalities");
        }

        var operand = astNodes[0];

        foreach (var substitution in set)
        {
            operand = substitute.Execute(operand, substitution);
        }

        return operand;
    }
}
