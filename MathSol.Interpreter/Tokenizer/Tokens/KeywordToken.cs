using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens;

public class KeywordToken(string value) : IToken
{
    public string Value { get; } = value;

    public override string ToString()
    {
        return $"[Keyword {Value}]";
    }
}
