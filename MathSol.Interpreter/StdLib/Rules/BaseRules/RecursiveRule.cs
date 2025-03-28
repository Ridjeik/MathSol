using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.BaseRules;

internal abstract class RecursiveRule([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct) : INodeRule
{
    public IAstNode Execute(IAstNode node)
    {
        if (node is not IOperatorAstNode operatorNode)
            return ExecuteRecursive(node);

        var updatedOperands = operatorNode.Operands.Select(Execute).ToList();
        
        if (updatedOperands.SequenceEqual(operatorNode.Operands))
            return ExecuteRecursive(node);
        
        var operandsSet = new SetNode(updatedOperands);
        var updatedNode = construct.Execute(new StringNode(operatorNode.Operator), operandsSet);
        
        return ExecuteRecursive(updatedNode);
    }

    protected abstract IAstNode ExecuteRecursive(IAstNode node);
}
