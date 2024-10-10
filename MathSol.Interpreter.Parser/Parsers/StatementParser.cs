using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class StatementParser : IParser
{
    private static readonly ProcedureParser ProcedureParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        return ProcedureParser.Parse(tokens);
    }
}
