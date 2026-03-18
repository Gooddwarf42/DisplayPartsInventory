using Microsoft.Extensions.DependencyInjection;
using WF.Data.Context;

namespace WF.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData<TDbContextConfigurator>(this IServiceCollection services)
        where TDbContextConfigurator : class, IDbContextConfigurator
        => services
            .AddScoped<ApplicationDbContext>()
            .AddScoped<IDbContextConfigurator, TDbContextConfigurator>();
}