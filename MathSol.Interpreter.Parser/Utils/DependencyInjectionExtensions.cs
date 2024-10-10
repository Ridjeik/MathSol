using MathSol.Interpreter.Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Parser.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddParser(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services.AddSingleton<IParser, Parser>();
    }
}
