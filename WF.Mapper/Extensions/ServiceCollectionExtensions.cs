using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WF.Mapper.Configurators;

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

        var mappingConfigurations = assembly.DefinedTypes
            .Where(type => type.IsAssignableTo(typeof(IMappingConfiguration)))
            .Where(type => type is { IsInterface: false, IsAbstract: false, });

        foreach (var mappingConfiguration in mappingConfigurations)
        {
            source.AddTransient(typeof(IMappingConfiguration), mappingConfiguration);
        }

        return source;
    }
}