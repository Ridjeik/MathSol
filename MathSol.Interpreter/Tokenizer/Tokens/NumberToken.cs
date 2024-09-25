using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens;

public class NumberToken(string value) : IToken
{
    public string Value => value;

    public override string ToString()
    {
        return $"[Number {Value}]";
    }
}
