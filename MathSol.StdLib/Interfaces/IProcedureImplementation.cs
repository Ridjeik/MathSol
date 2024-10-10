using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IProcedureImplementation
{
    public string FunctionName { get; }

    public IAstNode Execute(params IAstNode[] astNodes);
}