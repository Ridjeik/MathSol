using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal abstract class OneCharTokenReader<T> : ITokenReader
    where T : OneCharToken, new()
{
    private static char OperationChar => new T().TokenChar;

    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.PeekChar() == OperationChar)
        {
            codeFile.ConsumeChar();
            return new T();
        }

        return null;
    }
}
