using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class PlusMinusParser : IParser
{
    private static readonly MultiplicationDivisionParser MultiplicationDivisionParser = new MultiplicationDivisionParser();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var node = MultiplicationDivisionParser.Parse(tokens);

        if (tokens.Current is PlusToken or MinusToken)
        {
            var operation = tokens.Current;

            tokens.SkipAny();

            node = operation switch
            {
                PlusToken => new AdditionNode(node, this.Parse(tokens)),
                MinusToken => new SubtractionNode(node, this.Parse(tokens)),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }
}
