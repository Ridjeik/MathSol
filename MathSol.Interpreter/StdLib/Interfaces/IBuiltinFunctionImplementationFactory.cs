namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IBuiltinFunctionImplementationFactory
{
    IEnumerable<string> GetAllFunctionsNames();

    IEnumerable<IBuiltinFunctionImplementation> GetAllFunctionImplementations();
    IBuiltinFunctionImplementation GetFunctionImplementation(string procedureName);
}
