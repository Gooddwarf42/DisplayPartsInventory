using Cqrs.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Tests.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddCqrs
            (
                context =>
                    context.AddAssembly(typeof(ServiceCollectionExtensions))
            );
}