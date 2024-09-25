using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class KeywordTokenReader : ITokenReader
{
    private static readonly string[] Keywords = [
        "simplify"
    ];

    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        foreach (var word in Keywords)
        {
            var topChars = string.Empty;
            try
            {
                topChars = codeFile.PeekChars(word.Length);
            }
            catch (EndOfStreamException)
            {
                continue;
            }

            if (topChars.Equals(word, StringComparison.InvariantCultureIgnoreCase))
            {
                codeFile.ConsumeChars(word.Length);
                return new KeywordToken(word.ToLower());
            }
        }

        return null;
    }
}
