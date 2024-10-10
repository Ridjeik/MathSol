using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Tokenizer.Interface;

internal interface ITokenReader
{
    IToken? TryGetTokenFromCodeFile(CodeFile codeFile);
}
