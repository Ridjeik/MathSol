using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Utils;

internal static class TokensEnumeratorExtensions
{
    public static void Skip<TToken>(this IEnumerator<IToken> tokens)
            where TToken : IToken
    {
        if (tokens.Current is not TToken)
        {
            throw new InvalidOperationException($"Expected {typeof(TToken)}, but got {tokens.Current.GetType().Name}");
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

    public static void SkipAnyOf(this IEnumerator<IToken> tokens, params Type[] types)
    {
        if (!types.Any(t => t.IsInstanceOfType(tokens.Current)))
        {
            throw new InvalidOperationException();
        }

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

    public static IToken ConsumeAny(this IEnumerator<IToken> tokens)
    {
        var token = tokens.Current;
        tokens.MoveNext();
        return token;
    }

    public static IToken ConsumeAnyOf(this IEnumerator<IToken> tokens, params Type[] types)
    {
        if (!types.Any(t => t.IsInstanceOfType(tokens.Current)))
        {
            throw new InvalidOperationException();
        }

        var token = tokens.Current;
        tokens.MoveNext();
        return token;
    }

    public static IEnumerable<IToken> ConsumeWhile(this IEnumerator<IToken> tokens, Func<IToken, bool> predicate)
    {
        while (predicate(tokens.Current))
        {
            yield return tokens.Current;
            tokens.MoveNext();
        }
    }
}
