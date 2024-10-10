using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ProgramParser : IParser
{
    private static readonly StatementParser StatementParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var stmt = StatementParser.Parse(tokens);
        
        if (tokens.Current is not null)
        {
            throw new InvalidOperationException();
        }

        return stmt;
    }
}
