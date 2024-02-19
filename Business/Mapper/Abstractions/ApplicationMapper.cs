using AutoMapper;
using Business.Mapper.Abstractions.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Mapper.Abstractions;

public class ApplicationMapper : AutoMapper.Mapper
{
    public ApplicationMapper(IServiceProvider serviceProvider) : base(new ProvaConfigurationProvider(serviceProvider))
    {
    }

    private class ProvaConfigurationProvider : MapperConfiguration
    {
        public ProvaConfigurationProvider(IServiceProvider serviceProvider) : base(cfg => ConfigureMapping(cfg, serviceProvider)) { }

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
