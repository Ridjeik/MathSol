using MathSol.Interpreter.Executor.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MathSol.Interpreter.Executor.Utils;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddExecutor(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        return services
            .AddSingleton<IExecutor, Executor>();
    }
}
