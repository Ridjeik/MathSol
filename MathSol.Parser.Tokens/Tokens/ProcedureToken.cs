using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class ProcedureToken(string value) : IToken
{
    public string Value { get; } = value;

    public override string ToString()
    {
        return $"[Function {Value}]";
    }
}
