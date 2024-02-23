using AutoMapper;

namespace Mapper.Configurators;

public abstract class MappingConfiguration<T1, T2> : IMappingConfiguration
{
    public void Configure(IMapperConfigurationExpression configuratorExpression)
    {
        var t1ToT2 = configuratorExpression.CreateMap<T1, T2>();
        Configure(t1ToT2);
        var t2ToT1 = configuratorExpression.CreateMap<T2, T1>();
        Configure(t2ToT1);
    }

    protected virtual void Configure(IMappingExpression<T1, T2> expression) { }

    protected virtual void Configure(IMappingExpression<T2, T1> expression) { }
}