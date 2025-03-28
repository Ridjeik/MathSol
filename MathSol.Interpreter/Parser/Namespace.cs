using MathSol.Interpreter.Parser.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Parser;

public class Namespace : INamespace
{
    private List<IIdentifierAstNode> _identifierObjects { get; } = new();

    public IEnumerable<IIdentifierAstNode> IdentifierObjects => _identifierObjects;

    public void AddIdentifierObject(IIdentifierAstNode identifierObject) => _identifierObjects.Add(identifierObject);

    public INamespace Copy()
    {
        var result = new Namespace();
        foreach (var identifierObject in _identifierObjects)
            result.AddIdentifierObject(identifierObject);
        return result;
    }
}
