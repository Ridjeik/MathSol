using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class EqualityParser(IServiceProvider serviceProvider) : IInternalParser
{
    private PlusMinusParser PlusMinusParser => serviceProvider.GetRequiredService<PlusMinusParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var left = PlusMinusParser.Parse(tokens, @namespace);

        if (tokens.Current is not EqualsToken)
        {
            return left;
        }

        tokens.Skip<EqualsToken>();
        var right = PlusMinusParser.Parse(tokens, @namespace);
        return new EqualityNode(left, right);
    }
}
