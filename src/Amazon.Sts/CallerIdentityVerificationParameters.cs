#nullable disable

using System;
using System.Runtime.Serialization;

using Carbon.Json;

namespace Amazon.Sts
{
    public sealed class CallerIdentityVerificationParameters
    {
        public CallerIdentityVerificationParameters() { }

        public CallerIdentityVerificationParameters(string url, JsonObject headers, string body)
        {
            Url     = url     ?? throw new ArgumentNullException(nameof(url));
            Headers = headers ?? throw new ArgumentNullException(nameof(headers));
            Body    = body    ?? throw new ArgumentNullException(nameof(body));
        }

        [DataMember(Name = "url", Order = 1)]
        public string Url { get; set; }

        // includes the Authentication Header    
        [DataMember(Name = "headers", Order = 2)]
        public JsonObject Headers { get; set; }

        // Always POST
        [DataMember(Name = "body", Order = 3)]
        public string Body { get; set; }
    }
}