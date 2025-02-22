using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Utils;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("free_of")]
[FunctionParametersCount(2)]
internal class FreeOfProcedure : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression", "operand"];

    protected override BooleanNode ExecuteImpl(params IAstNode[] astNodes)
    {
        var currentOperand = astNodes[0];
        var operandWeLookFor = astNodes[1];
        
        if (currentOperand.Equals(operandWeLookFor))
            return false;

        if (currentOperand is not IOperatorAstNode operatorAstNode)
            return true;

        return operatorAstNode.Operands.All(operand => ExecuteImpl(operand, operandWeLookFor));
    }
}
