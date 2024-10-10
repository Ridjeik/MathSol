namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IProcedureImplementationFactory
{
    IEnumerable<string> GetAllProcedureNames();

    IProcedureImplementation GetProcedureImplementation(string procedureName);
}
