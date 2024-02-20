using AutoMapper;

namespace Mapper.Configurators;

public interface IMappingConfiguration
{
    void Configure(IMapperConfigurationExpression configuratorExpression);
}