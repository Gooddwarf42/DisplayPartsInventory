using Data.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions;

public static class ServiceCollectionExtensions
{
    // TODO; Why does this not work? I had done with a neat trick to do the thing with runtime types, but I can't see why this wouldn't work.
    public static IServiceCollection AddData<TConfigureDbContext>(this IServiceCollection services)
        where TConfigureDbContext : class, IConfigureDbContext
        => services
            .AddScoped<IConfigureDbContext, TConfigureDbContext>();
}