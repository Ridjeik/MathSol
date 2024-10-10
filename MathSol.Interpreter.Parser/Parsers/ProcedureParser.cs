using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class ProcedureParser : IParser
{
    private static readonly PlusMinusParser PlusMinusParser = new();

    public IAstNode Parse(IEnumerator<IToken> tokens)
    {
        var action = tokens.Consume<ProcedureToken>().Value;
        var operand = PlusMinusParser.Parse(tokens);

        var actionNode = new ProcedureNode(action, operand);

        return actionNode;
    }
}
