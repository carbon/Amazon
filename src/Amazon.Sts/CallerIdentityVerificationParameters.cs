using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Amazon.Sts
{
    public sealed class CallerIdentityVerificationParameters
    {
#nullable disable
        public CallerIdentityVerificationParameters() { }
#nullable enable

        public CallerIdentityVerificationParameters(string url, IReadOnlyDictionary<string, string> headers, string body)
        {
            Url     = url     ?? throw new ArgumentNullException(nameof(url));
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Body    = body    ?? throw new ArgumentNullException(nameof(body));
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        // includes the Authentication Header

        [JsonPropertyName("headers")]
        public IReadOnlyDictionary<string, string> Headers { get; set; }

        // Always POST
        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}