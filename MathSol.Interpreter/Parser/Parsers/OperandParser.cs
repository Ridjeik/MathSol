using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace MathSol.Interpreter.Parser.Parsers;

internal class OperandParser(IServiceProvider serviceProvider) : IInternalParser
{
    private PlusMinusParser PlusMinusParser => serviceProvider.GetRequiredService<PlusMinusParser>();
    private SubprogramCallParser ProcedureCallParser => serviceProvider.GetRequiredService<SubprogramCallParser>();
    private EqualityParser EqualityParser => serviceProvider.GetRequiredService<EqualityParser>();

    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace) => tokens.Current switch
    {
        NumberToken => ParseNumber(tokens),
        IdentifierToken => ParseVariableFunctionOrProcedureCall(tokens, @namespace),
        LeftParenthesesToken => ParseAddition(tokens, @namespace),
        LeftCurlyBracketToken => ParseSet(tokens, @namespace),
        StringToken => ParseString(tokens),
        _ => throw new InvalidOperationException()
    };

    private static StringNode ParseString(IEnumerator<IToken> tokens)
    {
        var token = tokens.Consume<StringToken>();
        return new StringNode(token.Value);
    }

    private SetNode ParseSet(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        tokens.Skip<LeftCurlyBracketToken>();

        var operands = new List<IAstNode>();
        while (tokens.Current is not RightCurlyBracketToken)
        {
            operands.Add(EqualityParser.Parse(tokens, @namespace));

            if (tokens.Current is not (CommaToken or RightCurlyBracketToken))
                throw new InvalidOperationException();

            if (tokens.Current is CommaToken)
                tokens.Skip<CommaToken>();
        }
        tokens.Skip<RightCurlyBracketToken>();

        return new SetNode(operands);
    }

    private IAstNode ParseAddition(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        tokens.Skip<LeftParenthesesToken>();
        var result = PlusMinusParser.Parse(tokens, @namespace);
        tokens.Skip<RightParenthesesToken>();
        return result;
    }

    private static NumberNode ParseNumber(IEnumerator<IToken> tokens)
    {
        var token = tokens.Consume<NumberToken>();
        return new NumberNode(Convert.ToDecimal(token.Value, CultureInfo.InvariantCulture));
    }

    private static VariableNode ParseVariable(IEnumerator<IToken> tokens)
    {
        var token = tokens.Consume<IdentifierToken>();
        return new VariableNode(token.Value);
    }

    private IAstNode ParseVariableFunctionOrProcedureCall(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var obj = @namespace.GetIdentifierObject(tokens.Current.Value);

        if (obj is null or VariableNode)
            return ParseVariable(tokens);

        if (obj is FunctionNode)
            return ParseFunction(tokens, @namespace);

        return ProcedureCallParser.Parse(tokens, @namespace);
    }

    private IAstNode ParseFunction(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        var function = tokens.Consume<IdentifierToken>();

        if (tokens.Current is not LeftParenthesesToken)
            return new VariableNode(function.Value);

        tokens.Skip<LeftParenthesesToken>();

        var @params = new List<IAstNode>();
        while (tokens.Current is not RightParenthesesToken)
        {
            @params.Add(EqualityParser.Parse(tokens, @namespace));

            if (tokens.Current is not RightParenthesesToken)
                tokens.Skip<CommaToken>();
        }

        tokens.Skip<RightParenthesesToken>();

        return new FunctionCallNode(function.Value, @params);
    }
}
