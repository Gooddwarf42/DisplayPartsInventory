using AutoMapper;
using Mapper.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Mapper;

public class DefaultMapper(IServiceProvider serviceProvider) : AutoMapper.Mapper(new ProvaConfigurationProvider(serviceProvider))
{
    private class ProvaConfigurationProvider(IServiceProvider serviceProvider) : MapperConfiguration(cfg => ConfigureMapping(cfg, serviceProvider))
    {
        private static void ConfigureMapping(IMapperConfigurationExpression cfg, IServiceProvider serviceProvider)
        {
            var mappingConfigurators = serviceProvider.GetServices<IMappingConfiguration>();
            foreach (var mappingConfigurator in mappingConfigurators)
            {
                mappingConfigurator.Configure(cfg);
            }
        }
    }
}
