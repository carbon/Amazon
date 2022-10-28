using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Amazon.Ec2;

internal static class Ensure
{
    public static void ValueBetween(int argument, int min, int max, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument < min || argument > max)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, $"Must be between {min} and {max}. Was {argument}."), paramName);
        }
    }

    public static void AtLeast(int argument, int value, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument < value)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, $"Must be at least {value}. Was {argument}."), paramName);
        }
    }

    public static void NotEmpty<T>([NotNull] T[]? argument, [CallerArgumentExpression("argument")] string? paramName = null)
    {
        if (argument is null)
        {
            throw new ArgumentNullException(paramName);
        }

        if (argument.Length is 0)
        {
            throw new ArgumentException("Must not be empty", paramName);
        }
    }
}
