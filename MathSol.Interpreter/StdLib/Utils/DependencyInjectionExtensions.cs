using MathSol.Interpreter.StdLib.Attributes;
using MathSol.Interpreter.StdLib.Executors;
using MathSol.Interpreter.StdLib.Interfaces;
using MathSol.Interpreter.StdLib.Maxima;
using MathSol.Interpreter.StdLib.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.StdLib.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStdLib(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .RegisterRules()
            .AddSingleton<CoreSimplifiersExecutor>()
            .RegisterProcedureImplementation()
            .AddSingleton<IBuiltinFunctionImplementationFactory, BuiltInProcedureImplementationFactory>()
            .AddSingleton<IVariableScopeFactory, VariableScopeFactory>()
            .AddSingleton<INodeExecutor, ProgramExecutor>()
            .AddSingleton<MaximaProcess>();
    }

    private static IServiceCollection RegisterProcedureImplementation(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();

        var procedureImplementationTypes = assembly.GetTypes().Where(t => typeof(IBuiltinFunctionImplementation)
            .IsAssignableFrom(t) &&
            t.IsClass &&
            !t.IsAbstract);

        foreach (var type in procedureImplementationTypes)
        {
            services.AddSingleton(typeof(IBuiltinFunctionImplementation), type);
            services.AddKeyedSingleton(typeof(IBuiltinFunctionImplementation), type.GetFunctionName(), type);
        }

        return services;
    }

    private static IServiceCollection RegisterRules(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();

        var ruleTypes = assembly.GetTypes().Where(t => typeof(INodeRule)
            .IsAssignableFrom(t) &&
            t.IsClass &&
            !t.IsAbstract);

        foreach (var type in ruleTypes)
        {
            var ruleTypesAttributesValues = type.GetCustomAttributes<RuleTypeAttribute>()?.Select(t => t.RuleType) ?? throw new InvalidOperationException($"Failed to get RuleTypeAttributes for {type.Name}");
            services.AddSingleton(typeof(INodeRule), type);
            ruleTypesAttributesValues.ToList().ForEach(ruleType => services.AddKeyedSingleton(typeof(INodeRule), ruleType, type));
        }

        return services;
    }
}
