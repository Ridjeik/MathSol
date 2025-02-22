using MathSol.Interpreter.Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.Parser.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddParser(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services.RegisterParsers()
            .AddSingleton<IParser, Parser>();
    }

    private static IServiceCollection RegisterParsers(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();

        var tokenReaderTypes = assembly.GetTypes().Where(t => typeof(IInternalParser)
            .IsAssignableFrom(t) &&
            t.IsClass &&
            !t.IsAbstract);

        foreach (var type in tokenReaderTypes)
        {
            services.AddSingleton(type);
        }

        return services;
    }
}
