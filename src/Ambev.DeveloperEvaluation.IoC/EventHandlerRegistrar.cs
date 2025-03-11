using Ambev.DeveloperEvaluation.Common.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC;

/// <summary>
/// Responsible for automatically registering event handlers in the application's dependency injection container.
/// </summary>
public static class EventHandlerRegistrar
{
    /// <summary>
    /// Scans the application's assemblies and registers all event handlers implementing <see cref="IEventHandler{T}"/>.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    public static void Register(IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var eventHandlers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>)))
            .ToList();

        foreach (var handler in eventHandlers)
        {
            var eventType = handler.GetInterfaces().First(i => i.IsGenericType).GetGenericArguments().First();
            var handlerInterface = typeof(IEventHandler<>).MakeGenericType(eventType);
            services.AddScoped(handlerInterface, handler);
        }
    }
}
