using MathSol.Interpreter.Shared.Nodes.Interfaces;
using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Enums;
using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.StdLib.Executors;

public class CoreSimplifiersExecutor(IServiceProvider serviceProvider) : IRulesExecutor
{
    private readonly IEnumerable<INodeRule> CoreSimplificationRules = serviceProvider.GetKeyedServices<INodeRule>(RuleType.CoreSimplification).OrderByDescending(rule => rule.GetType().GetCustomAttribute<RulePriorityAttribute>()?.Priority ?? 0);

    public IAstNode ExecuteRules(IAstNode node)
    {
        bool hasChanged;
        do
        {
            hasChanged = false;

            foreach (var rule in CoreSimplificationRules)
            {
                var result = rule.Execute(node);

                if (!result.Equals(node))
                {
                    hasChanged = true;
                    node = result;
                }
            }

        } while (hasChanged);

        return node;
    }
}
