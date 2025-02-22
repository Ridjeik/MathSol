using MathSol.Interpreter.Executor.Interfaces;
using MathSol.Interpreter.Shared;
using MathSol.Interpreter.Shared.Nodes;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Executors;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Executor;

public class Executor(IBuiltinFunctionImplementationFactory builtinFunctionImplementationFactory,
                        IVariableScopeFactory variableScopeFactory,
                        CoreSimplifiersExecutor coreSimplifiersExecutor,
                        [FromKeyedServices("sequential_substitute")] IBuiltinFunctionImplementation substitute,
                        IVariableScope? parentScope = null) : IExecutor
{
    private IVariableScope VariableScope { get; } = variableScopeFactory.CreateScope(parentScope);

    public void Execute(IAstNode program)
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
        programNode.Statements.ToList().ForEach(ExecuteStatement);
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

    private void ExecuteStatement(IAstNode statement)
    {
        switch (statement)
        {
            case SubprogramCallNode functionNode:
                ExecuteSubprogramCall(functionNode);
                return;
            case AssignmentNode assignmentNode:
                VariableScope.SetVariable(assignmentNode.Left, coreSimplifiersExecutor.ExecuteRules(assignmentNode.Right));
                return;
            case IfNode ifNode:
                ExecuteIf(coreSimplifiersExecutor, ifNode);
                return;
            case SubProgramDefinitionNode subProgramDefinitionNode:
                VariableScope.SetVariable(new VariableNode(subProgramDefinitionNode.Name), subProgramDefinitionNode);
                return;
            default:
                throw new Exception("Invalid statement node");
        };
    }

    private void ExecuteIf(CoreSimplifiersExecutor coreSimplifiersExecutor, IfNode ifNode)
    {
        if (coreSimplifiersExecutor.ExecuteRules(ifNode.Condition) is BooleanNode booleanNode && booleanNode.Value)
        {
            ExecuteSubprogram(ifNode.IfBody);
        }
        else if (ifNode.ElseBody is not null)
        {
            ExecuteSubprogram(ifNode.ElseBody);
        }
    }

    private void ExecuteSubprogram(IAstNode node)
    {
        new Executor(builtinFunctionImplementationFactory, variableScopeFactory, coreSimplifiersExecutor, substitute, this.VariableScope).Execute(node);
        variableScopeFactory.SetCurrentScope(VariableScope);
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
            statements.Add(new AssignmentNode(param, value));
        }

        statements.Add(subProgramDefinitionNode.Body);
        ExecuteSubprogram(new ProgramNode(statements));
    }
}
