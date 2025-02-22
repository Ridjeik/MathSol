using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Parser.Interfaces;

public interface INamespace
{
    IEnumerable<IIdentifierAstNode> IdentifierObjects { get; }
    void AddIdentifierObject(IIdentifierAstNode identifierObject);

    IIdentifierAstNode? GetIdentifierObject(string identifier) => IdentifierObjects.FirstOrDefault(x => x.Name == identifier);
    INamespace Copy();
}
