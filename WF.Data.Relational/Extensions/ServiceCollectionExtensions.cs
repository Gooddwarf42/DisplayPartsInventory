using Microsoft.Extensions.DependencyInjection;
using WF.Data.Relational.Context;

namespace WF.Data.Relational.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData<TDbContextConfigurator>(this IServiceCollection services)
        where TDbContextConfigurator : class, IDbContextConfigurator
        => services
            .AddScoped<IDbContextConfigurator, TDbContextConfigurator>();
}