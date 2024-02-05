using Data.Infrastructure;
using Data.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusiness<TConfigureDbContext>(this IServiceCollection services)
        where TConfigureDbContext : class, IConfigureDbContext
        => services.
            AddData<TConfigureDbContext>();
}