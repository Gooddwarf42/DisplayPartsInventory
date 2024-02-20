using AutoMapper;
using Mapper.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Mapper;

public class DefaultMapper(IServiceProvider serviceProvider) : AutoMapper.Mapper(new DefaultConfigurationProvider(serviceProvider))
//NOTE: here the notes I had passed the serviceProvider.GetRequiredService as the serviceCtor parameter in the base constructor. Not sure if it is really needed thouh.
{
    private class DefaultConfigurationProvider(IServiceProvider serviceProvider) : MapperConfiguration(cfg => ConfigureMapping(cfg, serviceProvider))
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
