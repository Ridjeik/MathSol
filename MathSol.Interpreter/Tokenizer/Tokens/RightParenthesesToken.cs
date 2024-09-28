using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens;

internal class RightParenthesesToken : IToken
{
    public string Value => ")";

    public override string ToString()
    {
        return "[RightParentheses]";
    }
}
