using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class NumberToken(string value) : IToken
{
    public string Value => value;

    public override string ToString()
    {
        return $"[{Value}]";
    }
}
