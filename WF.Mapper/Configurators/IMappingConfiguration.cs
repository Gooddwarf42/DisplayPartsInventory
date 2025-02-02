using AutoMapper;

namespace WF.Mapper.Configurators;

public interface IMappingConfiguration
{
    void Configure(IMapperConfigurationExpression configuratorExpression);
}