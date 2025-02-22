using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("number_of_operands")]
[FunctionParametersCount(1)]
internal class NumberOfOperandsProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        if (astNodes[0] is not IOperatorAstNode operatorAstNode)
        {
            return new NumberNode(0);
        }

        return new NumberNode(operatorAstNode.Operands.Count());
    }
}
