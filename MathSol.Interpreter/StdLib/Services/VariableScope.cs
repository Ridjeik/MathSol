using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;
using System.Collections;

namespace MathSol.Interpreter.StdLib.Services;

internal class VariableScope : IVariableScope
{
    private readonly Dictionary<VariableNode, IAstNode> Variables = new();
    private readonly IEnumerable<IAstNode> ParentOverridedVariables = [];

    private IVariableScope? ParentScope { get; }

    public VariableScope(IVariableScope? parentScope = null)
    {
        parentScope?.ToList().ForEach(x => Variables.Add(x.Key, x.Value));
        ParentScope = parentScope;
    }

    private VariableScope(IVariableScope? parentScope, IEnumerable<IAstNode> parentOverridedVariables)
        : this(parentScope)
    {
        ParentOverridedVariables = parentOverridedVariables;
    }

    public IAstNode? GetVariable(VariableNode variable)
    {
        if (!Variables.TryGetValue(variable, out IAstNode? value))
        {
            return null;
        }

        return value;
    }
    public void SetVariable(VariableNode variable, IAstNode value)
    {
        Variables[variable] = value;
        if (!ParentOverridedVariables.Contains(variable) && ParentScope is not null && ParentScope.GetVariable(variable) is not null)
        {
            ParentScope.SetVariable(variable, value);
        }
    }

    public IEnumerator<KeyValuePair<VariableNode, IAstNode>> GetEnumerator()
    {
        return Variables.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Variables.GetEnumerator();
    }

    public IVariableScope WithFixed(IEnumerable<IAstNode> variables)
    {
        return new VariableScope(this, variables);
    }
}
