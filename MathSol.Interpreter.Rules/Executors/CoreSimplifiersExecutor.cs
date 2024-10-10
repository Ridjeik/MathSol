using MathSol.Interpreter.Rules.Enums;
using MathSol.Interpreter.Rules.Interfaces;
using MathSol.Interpreter.Shared.Nodes.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Rules.Executors;

public class CoreSimplifiersExecutor([FromKeyedServices(RuleType.CoreSimplification)] IEnumerable<INodeRule> rules) : IRulesExecutor
{
    private readonly IEnumerable<INodeRule> CoreSimplificationRules = rules;

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
