using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Mediator;

namespace WF.Cqrs.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Action<CqrsContext> configure)
    {
        var cqrsContext = new CqrsContext();
        configure(cqrsContext);

        services.AddTransient(typeof(IMediator), cqrsContext.MediatorType);
        services.AddSingleton(cqrsContext); //I need this to inject the cqrs configuration into the mediator
        cqrsContext.ScanAssemblies();

        foreach (var handlerType in cqrsContext.HandlerTypes)
        {
            services.AddTransient(handlerType); //Handlers are registered directly via the concrete name!
        }

        return services;
    }
}