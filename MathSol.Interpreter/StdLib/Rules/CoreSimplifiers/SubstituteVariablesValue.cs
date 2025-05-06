using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Rules.BaseRules;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Rules.CoreSimplifiers;

[RuleType(RuleType.CoreSimplification)]
[RulePriority(101)]
internal class SubstituteVariablesValue(IVariableScopeFactory variableScopeFactory,
                                        [FromKeyedServices("construct")] IBuiltinFunctionImplementation construct,
                                        [FromKeyedServices("sequential_substitute")] IBuiltinFunctionImplementation substitute) : RecursiveRule(construct)
{
    protected override IAstNode ExecuteRecursive(IAstNode node)
    {
        if (node is VariableNode variableNode)
        {
            var variableScope = variableScopeFactory.GetCurrentScope();
            IAstNode? value = variableScope.GetVariable(variableNode);

            if (value is FunctionDeclarationNode functionDeclarationNode)
            {
                return functionDeclarationNode.Body;
            }

            return value ?? variableNode;
        }

        if (node is FunctionCallNode functionCallNode)
        {
            var variableScope = variableScopeFactory.GetCurrentScope();
            IAstNode? value = variableScope.GetVariable(new VariableNode(functionCallNode.Function));
            if (value is not FunctionDeclarationNode func)
            {
                throw new Exception($"Function {functionCallNode.Function} not found");
            }

            if (func.Function.Parameters.Count() != functionCallNode.Arguments.Count())
            {
                throw new Exception($"Function {functionCallNode.Function} expects {func.Function.Parameters.Count()} arguments, but got {functionCallNode.Arguments.Count()}");
            }

            var paramsWithValues = func.Function.Parameters.Zip(functionCallNode.Arguments).Select(pair => new EqualityNode(pair.First, pair.Second));
            return substitute.Execute(func.Body, new SetNode(paramsWithValues));
        }

        if (node is DynamicFunctionCallNode dynamicFunctionCallNode)
        {
            var variableScope = variableScopeFactory.GetCurrentScope();
            IAstNode? trueFuncName = variableScope.GetVariable(new VariableNode(dynamicFunctionCallNode.Operator));
            IAstNode? value = variableScope.GetVariable(trueFuncName as VariableNode ?? throw new Exception("FATAL"));
            if (value is not FunctionDeclarationNode or SubProgramDefinitionNode)
            {
                throw new Exception($"Variable {dynamicFunctionCallNode.Operator} was expected to contain function!");
            }

            if (value is FunctionDeclarationNode func)
            {
                return new FunctionCallNode(func.Function.Name, dynamicFunctionCallNode.Operands);
            }

            if (value is SubProgramDefinitionNode subProgram)
            {
                return new SubprogramCallNode(subProgram.Name, [.. dynamicFunctionCallNode.Operands]);
            }

        }

        return node;
    }
}
