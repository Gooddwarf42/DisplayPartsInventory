using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WF.Mapper.Configurators;

namespace WF.Mapper;

public class DefaultMapper(IServiceProvider serviceProvider) : AutoMapper.Mapper(new DefaultConfigurationProvider(serviceProvider), serviceProvider.GetRequiredService)
{
    private sealed class DefaultConfigurationProvider(IServiceProvider serviceProvider) : MapperConfiguration(cfg => ConfigureMapping(cfg, serviceProvider))
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