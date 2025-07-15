namespace WebApiMedWaytoR.Extentions;

public interface ICopy
{
    public ICopy Copy();
}

public interface ICopy<out T> : ICopy where T : class, ICopy<T>
{
    ICopy ICopy.Copy()
    {
        return Copy();
    }

    public new T Copy();
}

public static class Copier
{
    public static T Copy<T>(T source) where T : class, ICopy<T>
    {
        return source.Copy();
    }

    public static ICopy Copy(ICopy source)
    {
        return source.Copy();
    }

    public static int Copy(int source)
    {
        return source;
    }

    public static decimal Copy(decimal source)
    {
        return source;
    }

    public static string Copy(string source)
    {
        return new string(source);
    }

    public static Guid Copy(Guid source)
    {
        return new Guid(source.ToByteArray());
    }

    public static DateTime Copy(DateTime source)
    {
        return new DateTime(source.Ticks, source.Kind);
    }
}