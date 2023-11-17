using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Amazon.Ec2;

internal static class Ensure
{
    public static void ValueBetween(int argument, int min, int max, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument < min || argument > max)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, $"Must be between {min} and {max}. Was {argument}."), paramName);
        }
    }

    public static void NotEmpty<T>([NotNull] T[]? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
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