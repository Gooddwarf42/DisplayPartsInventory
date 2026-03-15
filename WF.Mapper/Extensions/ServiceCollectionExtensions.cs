using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WF.Mapper.Configurators;
using WF.Utils.Extensions;

namespace WF.Mapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapper<TMapper>(this IServiceCollection source, Type typeInTargetAssembly)
        where TMapper : class, IMapper
        => source.AddMapper<TMapper>(typeInTargetAssembly.Assembly);

    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceCollection AddMapper<TMapper>(this IServiceCollection source, Assembly assembly)
        where TMapper : class, IMapper
    {
        source.AddTransient<IMapper, TMapper>();

        foreach (var mappingConfiguration in assembly.GetConcreteTypesExtending<IMappingConfiguration>())
        {
            source.AddTransient(typeof(IMappingConfiguration), mappingConfiguration);
        }

        return source;
    }
}