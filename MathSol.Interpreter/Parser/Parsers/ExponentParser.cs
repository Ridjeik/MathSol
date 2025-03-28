using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ExponentParser(IServiceProvider serviceProvider) : IInternalParser
{
    private OperandParser OperandParser => serviceProvider.GetRequiredService<OperandParser>();
    private UnaryPlusMinusParser UnaryPlusMinusParser => serviceProvider.GetRequiredService<UnaryPlusMinusParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var @base = OperandParser.Parse(tokens, @namespace);

        if (tokens.Current is ExponentToken)
        {
            tokens.Skip<ExponentToken>();
            return new ExponentNode(@base, UnaryPlusMinusParser.Parse(tokens, @namespace));
        }

        return @base;
    }
}
