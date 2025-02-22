using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class UnaryPlusMinusParser(IServiceProvider serviceProvider) : IInternalParser
{
    private ExponentParser ExponentParser => serviceProvider.GetRequiredService<ExponentParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        if (tokens.Current is PlusToken or MinusToken)
        {
            var token = tokens.ConsumeAny();
            return token switch
            {
                PlusToken => new UnaryPlusNode(ExponentParser.Parse(tokens, @namespace)),
                MinusToken => new UnaryMinusNode(ExponentParser.Parse(tokens, @namespace)),
                _ => throw new InvalidOperationException()
            };
        }

        return ExponentParser.Parse(tokens, @namespace);
    }
}
