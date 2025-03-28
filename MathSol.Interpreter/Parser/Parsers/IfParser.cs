using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class IfParser(IServiceProvider serviceProvider) : IInternalParser
{
    private EqualityParser EqualityParser => serviceProvider.GetRequiredService<EqualityParser>();
    private StatementParser StatementParser => serviceProvider.GetRequiredService<StatementParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        tokens.Skip<IfToken>();

        tokens.Skip<LeftParenthesesToken>();
        var condition = EqualityParser.Parse(tokens, @namespace);
        tokens.Skip<RightParenthesesToken>();

        var ifBody = StatementParser.Parse(tokens, @namespace.Copy());

        if (tokens.Current is ElseToken)
        {
            tokens.Skip<ElseToken>();

            var elseBody = StatementParser.Parse(tokens, @namespace.Copy());

            return new IfNode(condition, new ProgramNode([ifBody]), new ProgramNode([elseBody]));
        }

        return new IfNode(condition, new ProgramNode([ifBody]));
    }
}
