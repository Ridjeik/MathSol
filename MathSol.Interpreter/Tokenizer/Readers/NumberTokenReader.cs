using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using System.Text;
using System.Text.RegularExpressions;

namespace MathSol.Interpreter.Tokenizer;

internal partial class NumberTokenReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        StringBuilder stringBuilder = new();

        while (IsAcceptableChar(codeFile.PeekChar()))
        {
            stringBuilder.Append(codeFile.ConsumeChar());
        }

        if (stringBuilder.Length is 0)
            return null;

        string result = stringBuilder.ToString();

        if (!NumberRegex().IsMatch(result))
            return null;

        return new NumberToken(result);
    }

    private static bool IsAcceptableChar(char? symbol) => symbol is char c && (char.IsDigit(c) || c is '.');

    [GeneratedRegex(@"^\d+(\.\d+)?$")]
    private static partial Regex NumberRegex();
}
