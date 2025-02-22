using MathSol.Interpreter.Shared.Nodes.Interfaces;

namespace MathSol.Interpreter.Shared.Nodes
{
    public class FunctionNode(string name, IEnumerable<VariableNode> parameters) : BaseNode, IIdentifierAstNode
    {
        public string Name { get; } = name;
        public IEnumerable<VariableNode> Parameters { get; } = parameters;

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters)})";
        }
    }
}