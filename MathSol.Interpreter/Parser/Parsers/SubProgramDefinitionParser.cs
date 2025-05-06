using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class SubProgramDefinitionParser(IServiceProvider serviceProvider) : IInternalParser
{
    private StatementParser StatementParser => serviceProvider.GetRequiredService<StatementParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        tokens.Skip<DefineToken>();
        var identifier = tokens.Consume<IdentifierToken>();
        tokens.Skip<LeftParenthesesToken>();

        var parameters = new List<IAstNode>();
        while (tokens.Current is not RightParenthesesToken)
        {
            var parameter = tokens.Consume<IdentifierToken>();
            if (tokens.Current is LeftParenthesesToken)
            {
                tokens.Skip<LeftParenthesesToken>();
                var subParameters = new List<VariableNode>();
                while (tokens.Current is not RightParenthesesToken)
                {
                    var subParameter = tokens.Consume<IdentifierToken>();
                    subParameters.Add(new VariableNode(subParameter.Value));
                    if (tokens.Current is not RightParenthesesToken)
                        tokens.Skip<CommaToken>();
                }
                tokens.Skip<RightParenthesesToken>();
                parameters.Add(new FunctionNode(parameter.Value, subParameters));
            }
            else
                parameters.Add(new VariableNode(parameter.Value));
            if (tokens.Current is not RightParenthesesToken)
                tokens.Skip<CommaToken>();
        }

        tokens.Skip<RightParenthesesToken>();
        tokens.Skip<AsToken>();

        @namespace.AddIdentifierObject(new SubprogramNode(identifier.Value, parameters));
        var body = StatementParser.Parse(tokens, @namespace.Copy());

        return new SubProgramDefinitionNode(new SubprogramNode(identifier.Value, parameters), body);
    }
}
