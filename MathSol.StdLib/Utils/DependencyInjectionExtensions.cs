﻿using MathSol.Interpreter.StdLib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.StdLib.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddStdLib(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .RegisterProcedureImplementation()
            .AddSingleton<IProcedureImplementationFactory, BuiltInProcedureImplementationFactory>();
    }

    private static IServiceCollection RegisterProcedureImplementation(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();

        var tokenReaderTypes = assembly.GetTypes().Where(t => typeof(IProcedureImplementation)
            .IsAssignableFrom(t) &&
            t.IsClass &&
            !t.IsAbstract);

        foreach (var type in tokenReaderTypes)
        {
            var obj = ActivatorUtilities.CreateInstance(services.BuildServiceProvider(), type) as IProcedureImplementation ?? throw new InvalidOperationException($"Failed to create instance of {type.Name}");

            services.AddSingleton(obj);
            services.AddKeyedSingleton(obj.FunctionName, obj);
        }

        

        return services;
    }
}
