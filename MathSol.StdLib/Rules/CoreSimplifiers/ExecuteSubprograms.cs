using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Executors;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using MathSol.Interpreter.StdLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
[RulePriority(100)]
internal class ExecuteSubprograms([FromKeyedServices("construct")] IBuiltinFunctionImplementation construct,
                                 IBuiltinFunctionImplementationFactory builtinFunctionImplementationFactory,
                                 IVariableScopeFactory variableScopeFactory,
                                 IServiceProvider serviceProvider,
                                 [FromKeyedServices("substitute")] IBuiltinFunctionImplementation substitute) : RecursiveRule(construct)
{
    private CoreSimplifiersExecutor CoreSimplifiersExecutor => serviceProvider.GetRequiredService<CoreSimplifiersExecutor>();

    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is not SubprogramCallNode functionNode)
        {
            return node;
        }

        var declaration = variableScopeFactory.GetCurrentScope().GetVariable(new VariableNode(functionNode.Name)) ?? throw new Exception($"Procedure {functionNode.Name} not found");

        if (declaration is not SubProgramDefinitionNode subProgramDefinitionNode)
        {
            throw new Exception($"Variable {functionNode.Name} is not a procedure");
        }

        if (subProgramDefinitionNode.Body is ExternalCallNode)
        {
            return builtinFunctionImplementationFactory
                .GetFunctionImplementation(functionNode.Name)
                .Execute(functionNode.Operands.ToArray());
        }

        if (subProgramDefinitionNode.SubprogramSignature.Params.Count() != functionNode.Operands.Count())
        {
            throw new Exception($"Procedure {functionNode.Name} expects {subProgramDefinitionNode.SubprogramSignature.Params.Count()} arguments, but got {functionNode.Operands.Count()}");
        }

        var statements = new List<IAstNode>();
        foreach (var (param, value) in subProgramDefinitionNode.SubprogramSignature.Params.Zip(functionNode.Operands))
        {
            statements.Add(new AssignmentNode(param, value));
        }

        statements.Add(subProgramDefinitionNode.Body);
        var result = new ProgramExecutor(builtinFunctionImplementationFactory, variableScopeFactory, CoreSimplifiersExecutor, substitute, variableScopeFactory.GetCurrentScope().WithFixed(subProgramDefinitionNode.SubprogramSignature.Params)).Execute(new ProgramNode(statements));

        return result.result;
    }
}
