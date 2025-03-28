using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Shared.Tokens
{
    public abstract class KeywordToken : IToken
    {
        protected abstract string Keyword { get; }

        public string Value => Keyword;

        public override string ToString() => $"[Keyword {Keyword}]";
    }
}