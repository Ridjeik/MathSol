using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class VariableTokenReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.PeekChar() is not char c || !char.IsLetter(c))
            return null;

        return new VariableToken(codeFile.ConsumeChar()?.ToString() ?? throw new InvalidOperationException());
    }
}
