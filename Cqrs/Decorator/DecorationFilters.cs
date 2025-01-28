using Utils.Extensions;

namespace Cqrs.Decorator;

public static class DecorationFilters
{
    public static Func<Type, bool> All() => _ => true;

    public static Func<Type, bool> OfType<T>() => t => t.Extends<T>();

    public static Func<Type, bool> HasAttribute<T>()
        where T : Attribute
    {
        var attributeType = typeof(T);
        return t => t.CustomAttributes.Any(a => a.AttributeType == attributeType);
    }
}