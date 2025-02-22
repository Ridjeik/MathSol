using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.StdLib.Interfaces;

public interface IVariableScope : IEnumerable<KeyValuePair<VariableNode, IAstNode>>
{
    IAstNode? GetVariable(VariableNode variable);
    void SetVariable(VariableNode variable, IAstNode value);

    IVariableScope WithFixed(IEnumerable<VariableNode> variables);
}
