using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("log")]
[FunctionParametersCount(1)]
class LogarithmProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["x"];
    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        if (astNodes[0] is VariableNode variable && variable.Name == "e")
        {
            return new NumberNode(1);
        }

        return new SubprogramCallNode("log", [astNodes[0]]);
    }
}
