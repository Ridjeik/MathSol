using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IBuiltinFunctionImplementation
{
    string Name { get; }
    IEnumerable<string> Arguments { get; }

    public IAstNode Execute(params IAstNode[] astNodes);
}