using MathSol.Interpreter.Tokenizer.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MathSol.Interpreter.Tokenizer.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddTokenizer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .RegisterTokenReaders()
            .AddSingleton<ITokenizer, Tokenizer>();
    }

    private static IServiceCollection RegisterTokenReaders(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var assembly = Assembly.GetExecutingAssembly();

        var tokenReaderTypes = assembly.GetTypes().Where(t => typeof(ITokenReader)
            .IsAssignableFrom(t) &&
            t.IsClass &&
            !t.IsAbstract);

        foreach (var type in tokenReaderTypes)
        {
            services.AddSingleton(typeof(ITokenReader), type);
        }

        return services;
    }
}
