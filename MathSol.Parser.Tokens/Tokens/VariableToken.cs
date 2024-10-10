using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class VariableToken(string name) : IToken
{
    public string Value => Name;

    private string Name { get; } = name;

    public override string ToString() => $"[Variable {Name}]";
}
