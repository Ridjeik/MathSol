using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Interfaces;
using System.Reflection;

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

        public static int GetParametersCount(this Type procedureImplementationType)
        {
            if (!procedureImplementationType.IsAssignableTo(typeof(IBuiltinFunctionImplementation)))
            {
                throw new ArgumentException("Procedure implementation type must implement IProcedureImplementation interface");
            }

            return procedureImplementationType.GetCustomAttribute<FunctionParametersCountAttribute>()?.ParametersCount ?? 0;
        }

        public static string GetFunctionName(this Type procedureImplementationType)
        {
            if (!procedureImplementationType.IsAssignableTo(typeof(IBuiltinFunctionImplementation)))
            {
                throw new ArgumentException("Procedure implementation type must implement IProcedureImplementation interface");
            }

            return procedureImplementationType.GetCustomAttribute<FunctionNameAttribute>()?.Name
                ?? procedureImplementationType.Name.Replace("Procedure", string.Empty);
        }
    }
}