using AutoMapper;

namespace Business.Mapper.Abstractions.Configurators;

public interface IMappingConfiguration
{
    void Configure(IMapperConfigurationExpression configuratorExpression);
}