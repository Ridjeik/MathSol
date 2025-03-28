using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("cos")]
[FunctionParametersCount(1)]
class CosineProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["x"];
    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return new SubprogramCallNode("cos", [astNodes[0]]);
    }
}
