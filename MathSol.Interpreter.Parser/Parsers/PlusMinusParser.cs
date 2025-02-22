using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using MathSol.Interpreter.Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class PlusMinusParser(IServiceProvider serviceProvider) : IInternalParser
{
    private MultiplicationDivisionParser MultiplicationDivisionParser => serviceProvider.GetRequiredService<MultiplicationDivisionParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var node = MultiplicationDivisionParser.Parse(tokens, @namespace);

        if (tokens.Current is PlusToken or MinusToken)
        {
            var operation = tokens.Current;

            tokens.SkipAny();

            node = operation switch
            {
                PlusToken => new AdditionNode(node, this.Parse(tokens, @namespace)),
                MinusToken => new SubtractionNode(node, this.Parse(tokens, @namespace)),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }
}
