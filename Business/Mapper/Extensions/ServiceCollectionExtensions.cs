using Mapper;
using Mapper.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection source)
        => source.AddMapper<DefaultMapper>(typeof(ServiceCollectionExtensions));

}
