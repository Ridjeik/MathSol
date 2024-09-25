using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class PlusTokenReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.PeekChar() == '+')
        {
            codeFile.ConsumeChar();
            return new PlusToken();
        }

        return null;
    }
}
