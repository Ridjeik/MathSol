namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IVariableScopeFactory
{
    IVariableScope CreateScope(IVariableScope? parentScope = null);
    IVariableScope GetCurrentScope();
    void SetCurrentScope(IVariableScope scope);
}
