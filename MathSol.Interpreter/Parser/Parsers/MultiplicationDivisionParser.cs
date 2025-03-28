using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class MultiplicationDivisionParser(IServiceProvider serviceProvider) : IInternalParser
{
    private UnaryPlusMinusParser UnaryPlusMinusParser => serviceProvider.GetRequiredService<UnaryPlusMinusParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var node = UnaryPlusMinusParser.Parse(tokens, @namespace);

        while (tokens.Current is MultiplicationToken or DivisionToken)
        {
            var operation = tokens.Current;

            tokens.SkipAny();

            node = operation switch
            {
                MultiplicationToken => new MultiplicationNode(node, UnaryPlusMinusParser.Parse(tokens, @namespace)),
                DivisionToken => new DivisionNode(node, UnaryPlusMinusParser.Parse(tokens, @namespace)),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }
}
