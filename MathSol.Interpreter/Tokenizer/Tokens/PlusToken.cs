using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens;

public class PlusToken : IToken
{
    public string Value => "+";

    public override string ToString()
    {
        return $"[Plus]";
    }
}
