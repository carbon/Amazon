#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses
{
    public sealed class BouncedRecipient
    {
        [JsonPropertyName("status")]
        public string Status { get; init; }

        [JsonPropertyName("action")]
        public string Action { get; init; }

        [JsonPropertyName("diagnosticCode")]
        public string DiagnosticCode { get; init; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; init; }
    }
}
