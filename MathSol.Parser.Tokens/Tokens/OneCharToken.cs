using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public abstract class OneCharToken : IToken
{
    public string Value => TokenChar.ToString();

    public override string ToString() => $"[{TokenChar}]";

    public abstract char TokenChar { get; }
}
