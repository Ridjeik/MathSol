using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Parser.Utils;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.Shared.Tokens;
using MathSol.Interpreter.Shared.Tokens.Interfaces;

namespace MathSol.Interpreter.Parser.Parsers;

internal class VariableParser : IInternalParser
{
    public IAstNode Parse(IEnumerator<IToken> tokens, INamespace @namespace)
    {
        if (tokens.Current is not IdentifierToken identifier)
            throw new Exception("Expected identifier");

        tokens.Consume<IdentifierToken>();
        return new VariableNode(identifier.Value);
    }
}
