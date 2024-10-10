using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Nodes;

namespace MathSol.Interpreter.StdLib.Functions;

internal class PrintAstProcedure : ProcedureImplementation
{
    public override string FunctionName => "printAST";

    public override int NumberOfOperands => 1;

    protected override IAstNode ExecuteImpl (params IAstNode[] astNodes)
    {
        Console.WriteLine(astNodes[0]);

        return new UndefinedNode();
    }
}