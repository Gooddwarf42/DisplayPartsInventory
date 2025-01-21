using System.Reflection;
using AutoMapper;
using Mapper.Configurators;
using Microsoft.Extensions.DependencyInjection;

namespace Mapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMapper<TMapper>(this IServiceCollection source, Type typeInTargetAssembly)
        where TMapper : class, IMapper
        => source.AddMapper<TMapper>(typeInTargetAssembly.Assembly);

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