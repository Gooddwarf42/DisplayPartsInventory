namespace WF.Utils.Extensions;

public static class TypeExtensions
{
    public static bool Extends(this Type source, Type target)
        => source.IsAssignableTo(target);

    public static bool Extends<T>(this Type source)
        => source.Extends(typeof(T));
}