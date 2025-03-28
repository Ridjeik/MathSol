using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public class EndOfFileToken : IToken
{
    public string Value => "EOL";
}
