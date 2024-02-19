using Business.Mapper.Abstractions.Configurators;
using Business.Mapper.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper.Abstractions.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection source)
    {
        source.AddTransient<ApplicationMapper>();

        //TODO: scan assembly and add IMappingConfiguration services
        source.AddTransient<IMappingConfiguration, PartMappingConfiguration>();

        return source;
    }
}
