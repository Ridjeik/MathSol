using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("kind")]
[FunctionParametersCount(1)]
internal class KindProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        if (astNodes[0] is not IOperatorAstNode operatorAstNode)
        {
            return new UndefinedNode();
        }

        return new StringNode(operatorAstNode.Operator);
    }
}
