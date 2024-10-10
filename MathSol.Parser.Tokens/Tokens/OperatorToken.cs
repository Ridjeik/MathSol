using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens;

public abstract class OperatorToken : IToken
{
    public string Value => OperationChar.ToString();

    public override string ToString() => $"[{OperationChar}]";

    public abstract char OperationChar { get; }
}
