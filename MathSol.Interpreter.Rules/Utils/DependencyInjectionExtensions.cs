using MathSol.Interpreter.Rules.Attributes;
using MathSol.Interpreter.Rules.Executors;
using MathSol.Interpreter.Rules.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.Rules.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRulesProcessing(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .RegisterRules()
            .AddSingleton<CoreSimplifiersExecutor>();
    }

    private static IServiceCollection RegisterRules(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();
        var rules = assembly.GetTypes().Where(t => typeof(INodeRule).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var rule in rules)
        {
            var ruleTypes = rule.GetCustomAttributes<RuleTypeAttribute>().Select(attr => attr.RuleType);

            foreach (var type in ruleTypes)
            {
                services.AddKeyedSingleton(typeof(INodeRule), type, rule);
            }
        }

        return services;
    }
}
