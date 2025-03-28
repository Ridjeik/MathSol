using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("printAST")]
[FunctionParametersCount(1)]
internal class PrintAstProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];

    protected override IAstNode ExecuteImpl (params IAstNode[] astNodes)
    {
        Console.WriteLine(astNodes[0]);

        return new UndefinedNode();
    }
}