namespace MathSol.Interpreter.Tokenizer.Interface;

internal interface ITokenReader
{
    IToken? TryGetTokenFromCodeFile(CodeFile codeFile);
}
