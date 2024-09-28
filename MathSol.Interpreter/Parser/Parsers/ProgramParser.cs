using MathSol.Interpreter.Parser.AstNodes;
using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Tokenizer.Interface;
using MathSol.Interpreter.Tokenizer.Tokens;
using MathSol.Interpreter.Utils;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ProgramParser : IParser
{
    private static readonly IParser ExpressionParser = new ExpressionParser();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var action = tokens.Consume<KeywordToken>().Value;
        var operand = ExpressionParser.Parse(tokens);

        var actionNode = new ActionNode(action, operand);

        return actionNode;
    }
}
