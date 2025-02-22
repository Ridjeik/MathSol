using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class SubprogramCallParser(IServiceProvider serviceProvider) : IInternalParser
{
    private EqualityParser EqualityParser => serviceProvider.GetRequiredService<EqualityParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var procedure = tokens.Consume<IdentifierToken>().Value;
        
        if (tokens.Current is LeftParenthesesToken)
        {
            tokens.Skip<LeftParenthesesToken>();

            var parameters = new List<IAstNode>();

            while (tokens.Current is not RightParenthesesToken)
            {
                var parameter = EqualityParser.Parse(tokens, @namespace);
                parameters.Add(parameter);

                if (tokens.Current is not (CommaToken or RightParenthesesToken))
                    throw new InvalidOperationException($"Expected ',' or ')' but found {tokens.Current}");

                if (tokens.Current is CommaToken)
                    tokens.Skip<CommaToken>();
            }

            tokens.Skip<RightParenthesesToken>();

            return new SubprogramCallNode(procedure, [.. parameters]);
        }

        return new VariableNode(procedure);
    }
}
