using MathSol.Interpreter.FileSystem;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Tokenizer.Interface;

public interface ITokenizer
{
    IEnumerable<IToken> Tokenize(CodeFile codeFile);
}
