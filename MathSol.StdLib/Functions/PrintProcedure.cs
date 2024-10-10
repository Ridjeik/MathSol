using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MathSol.Interpreter.StdLib.Executors;

namespace MathSol.Interpreter.StdLib.Functions;

internal class PrintProcedure 
    (CoreSimplifiersExecutor coreSimplifiersExecutor,
    [FromKeyedServices("printAST")] IProcedureImplementation printAST) 
    : ProcedureImplementation
{
    public override string FunctionName => "print";

    public override int NumberOfOperands => 1;

    private CoreSimplifiersExecutor CoreSimplifiersExecutor { get; } = coreSimplifiersExecutor;

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        var parameter = astNodes[0];
        var output = CoreSimplifiersExecutor.ExecuteRules(parameter);
        return printAST.Execute(output);
    }
}
