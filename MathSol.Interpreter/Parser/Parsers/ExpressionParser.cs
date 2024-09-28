using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;
using MathSol.Interpreter.Utils;
using System.Globalization;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ExpressionParser : IParser
{
    private static readonly PlusMinusParser PlusMinusParser = new PlusMinusParser();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        if (tokens.Current is LeftParenthesesToken)
        {
            tokens.Skip<LeftParenthesesToken>();
            var expr = PlusMinusParser.Parse(tokens);
            tokens.Skip<RightParenthesesToken>();
            return expr;
        }

        return PlusMinusParser.Parse(tokens);
    }
}
