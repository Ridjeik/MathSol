using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Utils;

internal static class TokensEnumeratorExtensions
{
    public static void Skip<TToken>(this IEnumerator<IToken> tokens)
            where TToken : IToken
    {
        if (tokens.Current is not TToken)
        {
            throw new InvalidOperationException();
        }

        tokens.MoveNext();
    }

    public static void Skip(this IEnumerator<IToken> tokens, Type type)
    {
        if (!type.IsInstanceOfType(tokens.Current))
        {
            throw new InvalidOperationException();
        }

        tokens.MoveNext();
    }

    public static void SkipAny(this IEnumerator<IToken> tokens)
    {
        tokens.MoveNext();
    }

    public static TToken Consume<TToken>(this IEnumerator<IToken> tokens)
        where TToken : IToken
    {
        if (tokens.Current is not TToken a)
        {
            throw new InvalidOperationException();
        }
        var token = a;
        tokens.MoveNext();
        return token;
    }
}
