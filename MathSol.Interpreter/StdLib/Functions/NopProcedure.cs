using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("nop")]
[FunctionParametersCount(1)]
class NopProcedure : FunctionImplementation
{ 
    public override IEnumerable<string> Arguments => ["expression"];
    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        return astNodes[0];
    }
}
