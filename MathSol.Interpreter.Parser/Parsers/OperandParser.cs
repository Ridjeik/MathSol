using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using System.Globalization;

namespace MathSol.Interpreter.Parser.Parsers;

internal class OperandParser : IParser
{
    private static readonly PlusMinusParser PlusMinusParser = new();
    private static readonly ProcedureParser FunctionParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens) => tokens.Current switch
    {
        NumberToken => ParseNumber(tokens),
        ProcedureToken => ParseFunction(tokens),
        LeftParenthesesToken => ParseAddition(tokens),
        VariableToken => ParseVariable(tokens),
        _ => throw new NotImplementedException()
    };

    private static IAstNode ParseAddition(IEnumerator<IToken> tokens)
    {
        tokens.Skip<LeftParenthesesToken>();
        var result = PlusMinusParser.Parse(tokens);
        tokens.Skip<RightParenthesesToken>();
        return result;
    }

    private static NumberNode ParseNumber(IEnumerator<IToken> tokens)
    {
        var token = tokens.Consume<NumberToken>();
        var result = new NumberNode(Convert.ToDecimal(token.Value, CultureInfo.InvariantCulture));
        return result;
    }

    private static VariableNode ParseVariable(IEnumerator<IToken> tokens)
    {
        var token = tokens.Consume<VariableToken>();
        var result = new VariableNode(token.Value);
        return result;
    }

    private static IAstNode ParseFunction(IEnumerator<IToken> tokens)
    {
        return FunctionParser.Parse(tokens);
    }
}
