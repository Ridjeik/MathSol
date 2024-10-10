using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class UnaryPlusMinusParser : IParser
{
    private static readonly ExponentParser ExponentParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        if (tokens.Current is PlusToken or MinusToken)
        {
            var token = tokens.ConsumeAny();
            return token switch
            {
                PlusToken => new UnaryPlusNode(ExponentParser.Parse(tokens)),
                MinusToken => new UnaryMinusNode(ExponentParser.Parse(tokens)),
                _ => throw new InvalidOperationException()
            };
        }

        return ExponentParser.Parse(tokens);
    }
}
