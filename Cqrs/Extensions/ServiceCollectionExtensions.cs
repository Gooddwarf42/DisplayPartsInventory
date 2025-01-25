using Cqrs.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Action<CqrsConfiguration> configure)
    {
        var cqrsConfiguration = new CqrsConfiguration();
        configure(cqrsConfiguration);

        services.AddTransient(typeof(IMediator), cqrsConfiguration.MediatorType);
        // services.AddSingleton(cqrsConfiguration); //Why do I need this?
        cqrsConfiguration.ScanAssemblies();

        foreach (var handlerType in cqrsConfiguration.HandlerTypes)
        {
            services.AddTransient(handlerType); //Handlers are registered directly via the concrete name!
        }

        return services;
    }
}