using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MathSol.Interpreter.StdLib.Executors;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("print")]
[FunctionParametersCount(1)]
internal class PrintProcedure
    (IServiceProvider serviceProvider,
    [FromKeyedServices("printAST")] IBuiltinFunctionImplementation printAST) 
    : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];

    private CoreSimplifiersExecutor CoreSimplifiersExecutor => serviceProvider.GetRequiredService<CoreSimplifiersExecutor>();

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        var parameter = astNodes[0];
        var output = CoreSimplifiersExecutor.ExecuteRules(parameter);
        return printAST.Execute(output);
    }
}
