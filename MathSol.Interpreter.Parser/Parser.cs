using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Parsers;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser;

internal class Parser : IParser
{
    private readonly static ProgramParser ProgramParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        return ProgramParser.Parse(tokens);
    }
}
