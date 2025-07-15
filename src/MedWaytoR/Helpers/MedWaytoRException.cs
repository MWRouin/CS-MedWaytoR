using System.Diagnostics.CodeAnalysis;

namespace MWR.MedWaytoR.Helpers;

/// <summary>
/// You shouldn't either throw this exception or catch it.
/// if you see this exception in your code, it means that either you miss-used the library or that there is a bug in the library.
/// </summary>
public class MedWaytoRException : Exception
{
    private MedWaytoRException() : base("Something went wrong ! please report this issue !")
    {
    }

    private MedWaytoRException(string message) : base(message)
    {
    }

    private MedWaytoRException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #region Throw

    [DoesNotReturn]
    internal static object Throw()
    {
        throw new MedWaytoRException();
    }

    [DoesNotReturn]
    internal static T Throw<T>()

    {
        throw new MedWaytoRException();
    }

    [DoesNotReturn]
    internal static object Throw(string message)
    {
        throw new MedWaytoRException(message);
    }

    [DoesNotReturn]
    internal static T Throw<T>(string message)

    {
        throw new MedWaytoRException(message);
    }

    [DoesNotReturn]
    internal static object Throw(string message, Exception innerException)
    {
        throw new MedWaytoRException(message, innerException);
    }

    [DoesNotReturn]
    internal static T Throw<T>(string message, Exception innerException)

    {
        throw new MedWaytoRException(message, innerException);
    }

    #endregion

    // Predefined
    [DoesNotReturn]
    internal static object ThrowFailedToCreateInstanceOf(Type tExecutor)
    {
        throw new MedWaytoRException($"Failed to create an instance of {tExecutor.FullName}!");
    }
}