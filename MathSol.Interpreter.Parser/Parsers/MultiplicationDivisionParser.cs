using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class MultiplicationDivisionParser : IParser
{
    private static readonly UnaryPlusMinusParser UnaryPlusMinusParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var node = UnaryPlusMinusParser.Parse(tokens);

        while (tokens.Current is MultiplicationToken or DivisionToken)
        {
            var operation = tokens.Current;

            tokens.SkipAny();

            node = operation switch
            {
                MultiplicationToken => new MultiplicationNode(node, UnaryPlusMinusParser.Parse(tokens)),
                DivisionToken => new DivisionNode(node, UnaryPlusMinusParser.Parse(tokens)),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }
}
