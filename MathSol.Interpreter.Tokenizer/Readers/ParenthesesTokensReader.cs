using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Tokenizer.Readers;

internal class ParenthesesTokensReader : ITokenReader
{
    public IToken? TryGetTokenFromCodeFile(CodeFile codeFile)
    {
        if (codeFile.CurrentChar is '(')
        {
            codeFile.ConsumeChar();
            return new LeftParenthesesToken();
        }

        if (codeFile.CurrentChar is ')') 
        {
            codeFile.ConsumeChar();
            return new RightParenthesesToken();
        }

        return null;
    }
}
