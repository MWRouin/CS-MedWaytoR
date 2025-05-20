namespace MWR.MedWaytoR.Helpers;

internal static class ReflectionHelpers
{
    public static bool IsPublicConcreteClass(this Type type)
    {
        return type is { IsAbstract: false, IsInterface: false, IsPublic: true, IsGenericTypeDefinition: false };
    }

    public static Func<Type, bool> GenericTypeEqualityPredicate(this Type genericTypeDefinition)
    {
        ThrowIfNotGenericTypeDefinition(genericTypeDefinition);

        return type =>
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == genericTypeDefinition;
    }

    public static Func<Type, bool> GenericTypeEqualityPredicate(this Type genericTypeDefinition, params Type[] args)
    {
        var isGenericTypePredicate = genericTypeDefinition.GenericTypeEqualityPredicate();

        return type => isGenericTypePredicate(type) && type.GetGenericArguments().SequenceEqual(args);
    }

    private static void ThrowIfNotGenericTypeDefinition(Type type)
    {
        if (!type.IsGenericTypeDefinition)
            throw new InvalidOperationException(
                $"""
                 Type {type.Name} is not a generic type definition.
                 Expected a type like GenericTypeDefinition<,> and not GenericTypeDefinition<T1,T2>.
                 """);
    }
}