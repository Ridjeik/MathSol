using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class IdentifierToken(string name) : IToken
{
    public string Value => name;

    public override string ToString() => $"[Identifier {name}]";
}
