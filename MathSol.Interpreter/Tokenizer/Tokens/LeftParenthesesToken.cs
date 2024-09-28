using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens;

internal class LeftParenthesesToken : IToken
{
    public string Value => "(";

    public override string ToString()
    {
        return "[LeftParentheses]";
    }
}
