namespace MWR.MedWaytoR.Helpers;

internal static class ReflectionHelpers
{
    public static bool IsPublicConcreteClass(this Type type)
    {
        return type is { IsAbstract: false, IsInterface: false, IsPublic: true };
    }

    public static Func<Type, bool> GenericTypeEqualityPredicate(this Type genericInterface)
    {
        return @interface =>
            @interface.IsGenericType &&
            @interface.GetGenericTypeDefinition() == genericInterface;
    }

    public static Func<Type, bool> GenericTypeEqualityPredicate(this Type genericInterface, params Type[] args)
    {
        return @interface =>
            GenericTypeEqualityPredicate(genericInterface)(@interface) &&
            @interface.GetGenericArguments().SequenceEqual(args);
    }
}