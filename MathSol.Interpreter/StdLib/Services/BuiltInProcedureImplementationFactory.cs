using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Services;

internal class BuiltInProcedureImplementationFactory(IServiceProvider serviceProvider) : IBuiltinFunctionImplementationFactory
{
    private IEnumerable<IBuiltinFunctionImplementation> ProcedureImplementations => serviceProvider.GetService<IEnumerable<IBuiltinFunctionImplementation>>() ?? throw new InvalidOperationException("No procedures loaded");

    public IEnumerable<IBuiltinFunctionImplementation> GetAllFunctionImplementations() => ProcedureImplementations;

    public IEnumerable<string> GetAllFunctionsNames() => ProcedureImplementations.Select(pi => pi.GetType().GetFunctionName());

    public IBuiltinFunctionImplementation GetFunctionImplementation(string procedureName) => ProcedureImplementations.First(pi => pi.GetType().GetFunctionName() == procedureName);
}
