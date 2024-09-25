namespace MathSol.Interpreter.Tokenizer.Interface;

internal interface ITokenizer
{
    IEnumerable<IToken> Tokenize(CodeFile codeFile);
}
