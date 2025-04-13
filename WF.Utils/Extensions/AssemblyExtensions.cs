using System.Reflection;

namespace WF.Utils.Extensions;

public static class AssemblyExtensions
{
    public static IEnumerable<TypeInfo> GetConcreteTypesExtending<T>(this IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(t => t.Extends<T>())
            .Where(t => t is { IsAbstract: false, IsInterface: false });
}