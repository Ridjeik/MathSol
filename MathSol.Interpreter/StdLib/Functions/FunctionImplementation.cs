using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Utils;

namespace MathSol.Interpreter.StdLib.Functions
{
    internal abstract class FunctionImplementation : IBuiltinFunctionImplementation
    {
        public int ArgumentsCount => Arguments.Count();
        public string Name => this.GetType().GetFunctionName();
        public abstract IEnumerable<string> Arguments { get; }

        public IAstNode Execute(params IAstNode[] astNodes)
        {
            FunctionsUtil.EnsureArguments(astNodes, ArgumentsCount);

            return ExecuteImpl(astNodes);
        }

        protected abstract IAstNode ExecuteImpl(params IAstNode[] astNodes);
    }
}
