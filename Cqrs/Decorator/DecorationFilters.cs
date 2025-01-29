using Cqrs.Operations;
using Utils.Extensions;

namespace Cqrs.Decorator;

public static class DecorationFilters
{
    public static Func<Type, bool> All() => _ => true;

    public static Func<Type, bool> OfType<T>() => t => t.Extends<T>();
    public static Func<Type, bool> IsCommand() => t => t.Extends<IBaseCommand>();
    public static Func<Type, bool> IsQuery() => t => t.Extends<IBaseQuery>();
    public static Func<Type, bool> IsEvent() => t => t.Extends<IBaseEvent>();

    public static Func<Type, bool> HasAttribute<T>()
        where T : Attribute
    {
        var attributeType = typeof(T);
        return t => t.CustomAttributes.Any(a => a.AttributeType == attributeType);
    }
}