using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Utils;

namespace MathSol.Interpreter.StdLib.Functions
{
    internal abstract class ProcedureImplementation : IProcedureImplementation
    {
        public abstract string FunctionName { get; }

        public abstract int NumberOfOperands { get; }

        public IAstNode Execute(params IAstNode[] astNodes)
        {
            FunctionsUtil.EnsureArguments(astNodes, NumberOfOperands);

            return ExecuteImpl(astNodes);
        }

        protected abstract IAstNode ExecuteImpl(params IAstNode[] astNodes);
    }
}
