using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes;

public class SubProgramDefinitionNode(SubprogramNode subprogramNode, IAstNode body) : BaseNode, IIdentifierAstNode
{
    public SubprogramNode SubprogramSignature { get; } = subprogramNode;
    public IAstNode Body { get; } = body;

    public string Name => SubprogramSignature.Name;

    public override string ToString()
    {
        return $"{SubprogramSignature} {{{Body}}}";
    }
}
