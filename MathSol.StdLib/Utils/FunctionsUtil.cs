using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Utils
{
    internal static class FunctionsUtil
    {
        public static void EnsureArguments(IAstNode[] astNodes, int v)
        {
            if (astNodes.Length != v)
                throw new ArgumentException($"Function expects {v} arguments, but got {astNodes.Length}");
        }
    }
}