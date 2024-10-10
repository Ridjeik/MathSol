using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal abstract class OperatorTokenReader<T> : ITokenReader
    where T : IToken, new()
{
    public abstract char OperationChar { get; }

    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.PeekChar() == this.OperationChar)
        {
            codeFile.ConsumeChar();
            return new T();
        }

        return null;
    }
}
