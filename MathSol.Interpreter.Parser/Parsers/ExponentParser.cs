using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ExponentParser : IParser
{
    private static readonly OperandParser OperandParser = new();
    private static readonly PlusMinusParser PlusMinusParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var @base = OperandParser.Parse(tokens);

        if (tokens.Current is ExponentToken)
        {
            tokens.Skip<ExponentToken>();

            return new ExponentNode(@base, PlusMinusParser.Parse(tokens));
        }

        return @base;
    }
}
