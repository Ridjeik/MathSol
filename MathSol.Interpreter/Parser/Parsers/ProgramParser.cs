using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ProgramParser(IServiceProvider serviceProvider) : IInternalParser
{
    private StatementParser StatementParser => serviceProvider.GetRequiredService<StatementParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var statements = new List<IAstNode>();

        while (tokens.Current is not EndOfFileToken and not RightCurlyBracketToken) //TODO: Separate those logic into SubprogramParser
            statements.Add(StatementParser.Parse(tokens, @namespace));

        return new ProgramNode(statements);
    }
}
