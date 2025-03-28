using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class StringToken (string value) : IToken
{
    public string Value => value;
}
