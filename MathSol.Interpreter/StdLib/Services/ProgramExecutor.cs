using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Executors;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.StdLib.Services;

public class ProgramExecutor(IBuiltinFunctionImplementationFactory builtinFunctionImplementationFactory,
                        IVariableScopeFactory variableScopeFactory,
                        CoreSimplifiersExecutor coreSimplifiersExecutor,
                        [FromKeyedServices("sequential_substitute")] IBuiltinFunctionImplementation substitute,
                        IVariableScope? parentScope = null) : INodeExecutor
{
    private IVariableScope VariableScope { get; } = variableScopeFactory.CreateScope(parentScope);

    public (IAstNode result, bool isTerminated) Execute(IAstNode program)
    {
        if (program is not ProgramNode programNode)
        {
            throw new Exception("Invalid program node");
        }

        if (parentScope is null)
        {
            LoadBuiltins(VariableScope);
        }

        variableScopeFactory.SetCurrentScope(VariableScope);
        foreach (var statement in programNode.Statements)
        {
            if (statement is ReturnNode returnNode) 
                return (coreSimplifiersExecutor.ExecuteRules(returnNode.ReturnValue), true);

            var result = ExecuteStatement(statement);
            if (result.isTerminated)
            {
                return result;
            }
        }

        return (new UndefinedNode(), false);
    }

    private void LoadBuiltins(IVariableScope variableScope)
    {
        builtinFunctionImplementationFactory.GetAllFunctionImplementations().ToList().ForEach(function =>
        {
            variableScope.SetVariable(new VariableNode(function.Name), new SubProgramDefinitionNode(
                        new SubprogramNode(function.Name, function.Arguments.Select(arg => new VariableNode(arg))),
                        new ExternalCallNode()
                    ));
        });
    }

    private (IAstNode result, bool isTerminated) ExecuteStatement(IAstNode statement)
    {
        switch (statement)
        {
            case SubprogramCallNode functionNode:
                ExecuteSubprogramCall(functionNode);
                return (new UndefinedNode(), false);
            case AssignmentNode assignmentNode:
                VariableScope.SetVariable(assignmentNode.Left, coreSimplifiersExecutor.ExecuteRules(assignmentNode.Right));
                return (new UndefinedNode(), false);
            case FunctionDeclarationNode functionDeclarationNode:
                VariableScope.SetVariable(new VariableNode(functionDeclarationNode.Function.Name), functionDeclarationNode.WithBody(coreSimplifiersExecutor.ExecuteRules(functionDeclarationNode.Body)));
                return (new UndefinedNode(), false);
            case IfNode ifNode:
                var result = ExecuteIf(coreSimplifiersExecutor, ifNode);
                return result;
            case SubProgramDefinitionNode subProgramDefinitionNode:
                VariableScope.SetVariable(new VariableNode(subProgramDefinitionNode.Name), subProgramDefinitionNode);
                return (new UndefinedNode(), false);
            case ProgramNode programNode:
                var result2 = ExecuteSubprogram(programNode);
                return result2;
            default:
                throw new Exception("Invalid statement node");
        };
    }

    private (IAstNode result, bool isTerminated) ExecuteIf(CoreSimplifiersExecutor coreSimplifiersExecutor, IfNode ifNode)
    {
        if (coreSimplifiersExecutor.ExecuteRules(ifNode.Condition) is BooleanNode booleanNode && booleanNode.Value)
        {
            return ExecuteSubprogram(ifNode.IfBody);
        }
        else if (ifNode.ElseBody is not null)
        {
            return ExecuteSubprogram(ifNode.ElseBody);
        }

        return (new UndefinedNode(), false);
    }

    private (IAstNode result, bool isTerminated) ExecuteSubprogram(IAstNode node, IEnumerable<IAstNode>? fixedVariables = null)
    {
        var newVarScope = fixedVariables is not null ? VariableScope.WithFixed(fixedVariables) : VariableScope;
        var result = new ProgramExecutor(builtinFunctionImplementationFactory, variableScopeFactory, coreSimplifiersExecutor, substitute, newVarScope).Execute(node);
        variableScopeFactory.SetCurrentScope(VariableScope);
        return result;
    }

    private void ExecuteSubprogramCall(SubprogramCallNode functionNode)
    {
        if (VariableScope.GetVariable(new VariableNode(functionNode.Name)) is not SubProgramDefinitionNode subProgramDefinitionNode)
        {
            throw new Exception($"Procedure {functionNode.Name} not found");
        }

        if (subProgramDefinitionNode.Body is ExternalCallNode)
        {
            var result = builtinFunctionImplementationFactory.GetFunctionImplementation(functionNode.Name).Execute(functionNode.Operands.ToArray());
            return;
        }

        if (subProgramDefinitionNode.SubprogramSignature.Params.Count() != functionNode.Operands.Count())
        {
            throw new Exception($"Procedure {functionNode.Name} expects {subProgramDefinitionNode.SubprogramSignature.Params.Count()} arguments, but got {functionNode.Operands.Count()}");
        }

        var statements = new List<IAstNode>();
        foreach (var (param, value) in subProgramDefinitionNode.SubprogramSignature.Params.Zip(functionNode.Operands))
        {
            if (param is VariableNode variable)
            {
                statements.Add(new AssignmentNode(variable, value));
            }
            else if (param is FunctionNode function)
            {
                statements.Add(new FunctionDeclarationNode(function, value));
            }
            else
            {
                throw new Exception($"Invalid parameter type {param.GetType()}");
            }
        }

        if (subProgramDefinitionNode.Body is not ProgramNode programNode)
        {
            statements.Add(subProgramDefinitionNode.Body);
        }
        else
        {
            statements.AddRange(programNode.Statements);
        }

        ExecuteSubprogram(new ProgramNode(statements), subProgramDefinitionNode.SubprogramSignature.Params);
    }
}
