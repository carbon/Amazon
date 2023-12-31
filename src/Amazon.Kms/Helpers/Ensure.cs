using System.Globalization;
using System.Runtime.CompilerServices;

namespace Amazon.Kms;

internal static class Ensure
{
    public static void LengthBetween(string argument, int min, int max, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        ArgumentNullException.ThrowIfNull(argument);

        if (argument.Length < min || argument.Length > max)
        {
            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, $"Length must be between {min} and {max}. Was {argument}."), paramName);
        }
    }
}