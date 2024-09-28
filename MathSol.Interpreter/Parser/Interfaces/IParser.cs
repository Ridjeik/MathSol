using MathSol.Interpreter.Tokenizer.Interface;

namespace MathSol.Interpreter.Parser.Interfaces;

internal interface IParser
{
    public IAstNode Parse(IEnumerator<IToken> tokens);
}
