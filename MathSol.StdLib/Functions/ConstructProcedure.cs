using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Functions;

internal class ConstructProcedure : ProcedureImplementation
{
    public override string FunctionName => "construct";

    public override int NumberOfOperands => 2;

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        throw new NotImplementedException();
    }
}
