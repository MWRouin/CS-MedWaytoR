namespace MWR.MedWaytoR.Helpers;

internal static class ReflectionHelpers
{
    public static bool IsPublicConcreteClass(this Type type)
    {
        return type is { IsAbstract: false, IsInterface: false, IsPublic: true, IsGenericTypeDefinition: false };
    }

    #region GenericTypeDefinitionEqualityPredicate

    public static Func<Type, bool> GenericTypeDefinitionEqualityPredicate(this Type genericTypeDefinition)
    {
        ThrowIfNotGenericTypeDefinition(genericTypeDefinition);

        return type =>
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == genericTypeDefinition;
    }

    public static Func<Type, bool> GenericTypeDefinitionEqualityPredicate(this Type genericTypeDefinition,
        params Type[] args)
    {
        var isGenericTypePredicate = genericTypeDefinition.GenericTypeDefinitionEqualityPredicate();

        return type => isGenericTypePredicate(type) && type.GetGenericArguments().SequenceEqual(args);
    }

    public static Func<Type, bool> GenericTypeDefinitionEqualityPredicate<TGeneric>()
    {
        var genericType = typeof(TGeneric);
        if (genericType.IsGenericTypeDefinition) return genericType.GenericTypeDefinitionEqualityPredicate();
        var args = genericType.GetGenericArguments();
        var genericTypeDefinition = genericType.GetGenericTypeDefinition();
        return genericTypeDefinition.GenericTypeDefinitionEqualityPredicate(args);
    }

    public static Func<Type, bool> GenericTypeDefinitionEqualityPredicate<T1, T2>(this Type genericTypeDefinition)
    {
        return genericTypeDefinition.GenericTypeDefinitionEqualityPredicate(typeof(T1), typeof(T2));
    }

    public static Func<Type, bool> GenericTypeDefinitionEqualityPredicate<T1, T2, T3>(this Type genericTypeDefinition)
    {
        return genericTypeDefinition.GenericTypeDefinitionEqualityPredicate(typeof(T1), typeof(T2), typeof(T3));
    }

    private static void ThrowIfNotGenericTypeDefinition(Type type)
    {
        if (!type.IsGenericTypeDefinition)
            MedWaytoRException.Throw(
                $"""
                 Type {type.Name} is not a generic type definition.
                 Expected a type like GenericTypeDefinition<,> and not GenericTypeDefinition<T1,T2>.
                 """,
                new InvalidOperationException());
    }

    #endregion
}