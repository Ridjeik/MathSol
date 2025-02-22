using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using System.Text.RegularExpressions;

namespace MathSol.Interpreter.Tokenizer.Readers;

[TokenReaderPriority(1)]
internal class KeywordTokenReader : ITokenReader
{
    private static readonly Dictionary<string, Type> KeywordsToTypes = new Dictionary<string, Type>
    {
        { "if", typeof(IfToken) },
        { "else", typeof(ElseToken) },
        { "define", typeof(DefineToken) },
        { "as", typeof(AsToken) },
        { "return", typeof(ReturnToken) }
    };

    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        foreach (var kvp in KeywordsToTypes)
        {
            string? topChars;
            try
            {
                topChars = codeFile.PeekChars(kvp.Key.Length + 1);
            }
            catch (EndOfStreamException)
            {
                continue;
            }

            if (Regex.IsMatch(topChars, kvp.Key + "\\b", RegexOptions.IgnoreCase))
            {
                codeFile.ConsumeChars(kvp.Key.Length);
                return (IToken?)Activator.CreateInstance(kvp.Value);
            }
        }

        return null;    
    }
}
