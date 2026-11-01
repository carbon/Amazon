using System.Globalization;

namespace Amazon.CloudWatch.Serialization;

internal static class CloudWatchTimestamp
{
    // Unspecified times are assumed to already be UTC
    public static string Format(DateTime value)
    {
        var utc = value.Kind is DateTimeKind.Local ? value.ToUniversalTime() : value;

        return utc.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
    }
}
