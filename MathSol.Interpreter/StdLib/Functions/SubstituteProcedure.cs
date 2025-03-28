using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Functions;

[FunctionName("substitute")]
[FunctionParametersCount(2)]
internal class SubstituteProcedure([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : FunctionImplementation
{
    public override IEnumerable<string> Arguments => ["expression", "substiution"];

    protected override IAstNode ExecuteImpl(params IAstNode[] astNodes)
    {
        if (astNodes[1] is not EqualityNode { Left: var from, Right: var to })
        {
            throw new ArgumentException("Second argument must be an equality node.");
        }

        var current = astNodes[0];

        if (current.Equals(from))
            return to;

        if (current is not IOperatorAstNode operatorAstNode)
        {
            return current;
        }

        var operands = operatorAstNode.Operands.Select(operand => ExecuteImpl(operand, astNodes[1])).ToArray();
 
        if (operands.SequenceEqual(operatorAstNode.Operands))
        {
            return current;
        }

        var newNode = construct.Execute(new StringNode(operatorAstNode.Operator), new SetNode(operands));

        return newNode.Equals(from) ? to : newNode;
    }
}
