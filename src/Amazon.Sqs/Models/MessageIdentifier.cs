using System.Runtime.CompilerServices;

namespace Amazon.Sqs;

internal static class MessageIdentifier
{
    // MessageGroupId and MessageDeduplicationId
    private const int MaxLength = 128;

    public static void ThrowIfTooLong(string? value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is { Length: > MaxLength })
        {
            throw new ArgumentOutOfRangeException(paramName, value.Length, "Must be 128 characters or fewer");
        }
    }
}
