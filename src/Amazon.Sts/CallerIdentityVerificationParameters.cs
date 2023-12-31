using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sts;

public sealed class CallerIdentityVerificationParameters
{
    public CallerIdentityVerificationParameters() { }

    [SetsRequiredMembers]
    public CallerIdentityVerificationParameters(string url, IReadOnlyDictionary<string, string> headers, string body)
    {
        ArgumentException.ThrowIfNullOrEmpty(url);
        ArgumentNullException.ThrowIfNull(headers);
        ArgumentNullException.ThrowIfNull(body);

        Url = url;
        Headers = headers;
        Body = body;
    }

    [JsonPropertyName("url")]
    public required string Url { get; init; }

    // includes the Authentication Header

    [JsonPropertyName("headers")]
    public required IReadOnlyDictionary<string, string> Headers { get; init; }

    // application/x-www-form-urlencoded encoded
    [JsonPropertyName("body")]
    public required string Body { get; init; }

    public TimeSpan GetAge()
    {
        DateTime date = DateTime.ParseExact(
            s        : Headers["x-amz-date"],
            format   : "yyyyMMddTHHmmssZ",
            provider : CultureInfo.InvariantCulture,
            style    : DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
        );

        TimeSpan age = DateTime.UtcNow - date;

        return (age < TimeSpan.Zero) ? TimeSpan.Zero : age;
    }
}