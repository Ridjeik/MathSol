using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;
using System.Globalization;

namespace MathSol.Interpreter.Parser;

internal class Parser : IParser
{
    private IEnumerator<IToken> Tokens { get; }
    private IToken CurrentToken => Tokens.Current;

    public Parser(IEnumerable<IToken> tokens)
    {
        Tokens = tokens.GetEnumerator();
        Tokens.MoveNext();
    }

    public IAstNode Parse()
    {
        return ParseProgram();
    }

    private IAstNode ParseProgram()
    {
        var action = Consume<KeywordToken>().Value;
        var actionNode = new ActionNode(action, ParseExpression());
        return actionNode;
    }

    private IAstNode ParseExpression()
    {
        if (CurrentToken is LeftParenthesesToken)
        {
            Skip<LeftParenthesesToken>();
            var expr = ParsePlusOrMinusExpression();
            Skip<RightParenthesesToken>();
            return expr;
        }

        return ParsePlusOrMinusExpression();
    }

    private IAstNode ParsePlusOrMinusExpression()
    {
        var node = ParseUnaryPlusOrMinus();

        while (CurrentToken is PlusToken)
        {
            var operation = CurrentToken;

            Skip(operation.GetType());

            node = operation switch
            {
                PlusToken => new PlusNode(node, ParseOperand()),
                _ => throw new InvalidOperationException()
            };
        }

        return node;
    }
    
    private IAstNode ParseUnaryPlusOrMinus()
    {
        if (CurrentToken is not PlusToken)
        {
            return ParseOperand();
        }

        var operation = CurrentToken;
        Skip(operation.GetType());

        return new UnaryPlusNode(ParseUnaryPlusOrMinus());
    }

    private IAstNode ParseOperand()
    {
        if (CurrentToken is LeftParenthesesToken)
        {
            return ParseExpression();
        }

        if (CurrentToken is NumberToken)
        {
            var result = new NumberNode(Convert.ToDecimal(CurrentToken.Value, CultureInfo.InvariantCulture));
            Skip<NumberToken>();
            return result;
        }

        throw new NotImplementedException();
    }

    private void Skip<TToken>()
        where TToken : IToken
    {
        if (Tokens.Current is not TToken)
        {
            throw new InvalidOperationException();
        }

        Tokens.MoveNext();
    }

    private void Skip(Type type)
    {
        if (!type.IsInstanceOfType(Tokens.Current))
        {
            throw new InvalidOperationException();
        }

        Tokens.MoveNext();
    }

    private TToken Consume<TToken>()
        where TToken : IToken
    {
        if (Tokens.Current is not TToken a)
        {
            throw new InvalidOperationException();
        }
        var token = a;
        Tokens.MoveNext();
        return token;
    }
}
