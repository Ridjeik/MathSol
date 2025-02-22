using MathSol.Interpreter.StdLib.Interfaces;

namespace MathSol.Interpreter.StdLib.Services;

internal class VariableScopeFactory : IVariableScopeFactory
{
    private IVariableScope? CurrentScope { get; set; } = null;

    public IVariableScope CreateScope(IVariableScope? parentScope = null) => new VariableScope(parentScope);

    public void SetCurrentScope(IVariableScope scope) => CurrentScope = scope;

    public IVariableScope GetCurrentScope() => CurrentScope ?? throw new Exception("Current scope is not set");
}
