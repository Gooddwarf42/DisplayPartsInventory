using AutoMapper;
using Business.Mapper.Configurators;
using Data.Dtos;
using Data.Entities;

namespace Business.Mapper.Abstractions;

public class ApplicationMapper : AutoMapper.Mapper
{
    public ApplicationMapper() : base(new ProvaConfigurationProvider())
    {
    }

    private class ProvaConfigurationProvider : MapperConfiguration
    {
        public ProvaConfigurationProvider() : base(cfg => ConfigureMapping(cfg)) { }

        private static void ConfigureMapping(IMapperConfigurationExpression cfg)
        {
            //Configure part
            var testConfigurator = new PartMappingConfiguration();
            testConfigurator.Configure(cfg);
        }
    }
}
