using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Tokens
{
    public class MinusToken : IToken
    {
        public string Value => "-";

        public override string ToString()
        {
            return $"[Minus]";
        }
    }
}
