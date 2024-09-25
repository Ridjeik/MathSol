using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;

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
