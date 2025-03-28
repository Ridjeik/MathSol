using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("sin")]
[FunctionParametersCount(1)]
class SineProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["x"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return new SubprogramCallNode("sin", [astNodes[0]]);
    }
}
