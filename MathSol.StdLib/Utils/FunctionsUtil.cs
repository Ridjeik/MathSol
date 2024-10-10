using MathSol.Interpreter.Shared.Nodes;
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

        public static void EnsureSet(SetNode setNode, int v)
        {
            if (setNode.Operands.Count() != v)
                throw new ArgumentException($"Set expected to contain {v} elements, but it contains {setNode.Operands.Count()}");
        }
    }
}