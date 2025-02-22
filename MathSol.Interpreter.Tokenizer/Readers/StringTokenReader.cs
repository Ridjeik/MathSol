using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class StringTokenReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.CurrentChar != '"' || !codeFile.RestOfCode.Skip(1).Contains('"'))
        {
            return null;
        }

        codeFile.ConsumeChar();

        var str = codeFile.ConsumeUntil(c => c != '"');

        codeFile.ConsumeChar();

        return new StringToken(str);
    }
}
