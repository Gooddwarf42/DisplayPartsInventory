using Cqrs.Extensions;
using Data.Extensions;
using Data.Infrastructure;
using Mapper;
using Mapper.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusiness<TConfigureDbContext>(this IServiceCollection services)
        where TConfigureDbContext : class, IConfigureDbContext
        => services
            .AddData<TConfigureDbContext>()
            .AddMapper<DefaultMapper>(typeof(ServiceCollectionExtensions))
            .AddCqrs
            (
                cqrsContext =>
                    cqrsContext.AddAssembly(typeof(ServiceCollectionExtensions))
            );
}