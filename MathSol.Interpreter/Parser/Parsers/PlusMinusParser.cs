using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;
using MathSol.Interpreter.Utils;
using System.Globalization;

namespace MathSol.Interpreter.Parser.Parsers;

internal class PlusMinusParser : IParser
{
    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var node = ParseUnaryPlusOrMinus(tokens);

        while (tokens.Current is PlusToken or MinusToken)
        {
            var operation = tokens.Current;

            tokens.SkipAny();

            node = operation switch
            {
                PlusToken => new PlusNode(node, ParseOperand(tokens)),
                MinusToken => new PlusNode(node, new MultiplyNode(new NumberNode(-1), ParseOperand(tokens))),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }

    private IAstNode ParseUnaryPlusOrMinus(IEnumerator<IToken> tokens)
    {
        if (tokens.Current is not (PlusToken or MinusToken))
        {
            return ParseOperand(tokens);
        }

        var operation = tokens.Current;
        tokens.Skip(operation.GetType());

        return operation switch
        {
            PlusToken => ParseOperand(tokens),
            MinusToken => new MultiplyNode(new NumberNode(-1), ParseOperand(tokens)),
            _ => throw new InvalidOperationException()
        };
    }

    private IAstNode ParseOperand(IEnumerator<IToken> tokens)
    {
        if (tokens.Current is not NumberToken)
        {
            return ParseExpression();
        }

        var result = new NumberNode(Convert.ToDecimal(tokens.Current.Value, CultureInfo.InvariantCulture));
        tokens.Skip<NumberToken>();
        return result;
    }
}
