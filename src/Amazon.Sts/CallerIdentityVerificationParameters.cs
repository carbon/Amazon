using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sts;

public sealed class CallerIdentityVerificationParameters
{
#nullable disable
    public CallerIdentityVerificationParameters() { }
#nullable enable

    public CallerIdentityVerificationParameters(string url!!, IReadOnlyDictionary<string, string> headers!!, string body!!)
    {
        Url = url;
        Headers = headers;
        Body = body;
    }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    // includes the Authentication Header

    [JsonPropertyName("headers")]
    public IReadOnlyDictionary<string, string> Headers { get; set; }

    // Always POST
    [JsonPropertyName("body")]
    public string Body { get; set; }

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