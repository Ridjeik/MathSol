using MathSol.Interpreter.StdLib.Interfaces;

namespace MathSol.Interpreter.StdLib;

internal class BuiltInProcedureImplementationFactory (IEnumerable<IProcedureImplementation> procedureImplementations) : IProcedureImplementationFactory
{
    private IEnumerable<IProcedureImplementation> ProcedureImplementations { get; } = procedureImplementations;

    public IEnumerable<string> GetAllProcedureNames() => ProcedureImplementations.Select(pi => pi.FunctionName);

    public IProcedureImplementation GetProcedureImplementation(string procedureName) => ProcedureImplementations.First(pi => pi.FunctionName == procedureName);
}
