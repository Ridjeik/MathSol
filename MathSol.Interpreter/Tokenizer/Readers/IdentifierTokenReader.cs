using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using System.Text.RegularExpressions;

namespace MathSol.Interpreter.Tokenizer.Readers;

//[TokenReaderPriority(2)]
internal partial class IdentifierTokenReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        var result = IdentifierRegex().Match(codeFile.RestOfCode);
        if (result.Success)
        {
            codeFile.ConsumeChars(result.Length);
            return (IdentifierToken?)new IdentifierToken(result.Value);
        }
        else
        {
            return null;
        }
    }

    [GeneratedRegex(@"^[a-zA-Z_]\w*")]
    private static partial Regex IdentifierRegex();
}
