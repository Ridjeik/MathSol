using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("type")]
[FunctionParametersCount(1)]
class TypeProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];
    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return new StringNode(astNodes[0].GetType().Name);
    }
}
