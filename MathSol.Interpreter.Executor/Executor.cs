using MathSol.Interpreter.Executor.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;

namespace MathSol.Interpreter.Executor;

internal class Executor(IProcedureImplementationFactory procedureImplementationFactory) : IExecutor
{
    private IProcedureImplementationFactory ProcedureImplementationFactory { get; } = procedureImplementationFactory;

    public void Execute(IAstNode program)
    {
        if (program is ProcedureNode functionNode)
        {
            this.ExecuteFunction(functionNode);
        }
    }

    private void ExecuteFunction(ProcedureNode functionNode)
    {
        ProcedureImplementationFactory.GetProcedureImplementation(functionNode.ProcedureName).Execute(functionNode.Operands.ToArray());
    }
}
