using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class AssignmentParser(IServiceProvider serviceProvider) : IInternalParser
{
    private EqualityParser EqualityParser => serviceProvider.GetRequiredService<EqualityParser>();
    private PlusMinusParser PlusMinusParser => serviceProvider.GetRequiredService<PlusMinusParser>();
    private VariableParser VariableParser => serviceProvider.GetRequiredService<VariableParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var variable = tokens.Consume<IdentifierToken>();

        if (tokens.Current is LeftParenthesesToken)
        {
            tokens.Skip<LeftParenthesesToken>();
            var @params = new List<VariableNode>();

            while (tokens.Current is not RightParenthesesToken)
            {
                @params.Add(VariableParser.Parse(tokens, @namespace) as VariableNode ?? throw new Exception("FATAL ERROR"));
                if (tokens.Current is not RightParenthesesToken)
                    tokens.Skip<CommaToken>();
            }

            tokens.Skip<RightParenthesesToken>();
            tokens.Skip<ColumnToken>();
            tokens.Skip<EqualsToken>();
            var body = PlusMinusParser.Parse(tokens, @namespace);

            return new FunctionDeclarationNode(new FunctionNode(variable.Value, @params), body);

        }

        tokens.Skip<ColumnToken>();
        tokens.Skip<EqualsToken>();
        var value = EqualityParser.Parse(tokens, @namespace);

        return new AssignmentNode(new VariableNode(variable.Value), value);
    }
}
