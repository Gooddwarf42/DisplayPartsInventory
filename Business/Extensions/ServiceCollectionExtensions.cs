using Data.Extensions;
using Data.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Extensions;
using WF.Mapper;
using WF.Mapper.Extensions;

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