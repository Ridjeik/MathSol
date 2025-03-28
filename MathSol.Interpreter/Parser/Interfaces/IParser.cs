using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Interfaces;

public interface IParser
{
    public IAstNode Parse(IEnumerator<IToken> tokens);
}
