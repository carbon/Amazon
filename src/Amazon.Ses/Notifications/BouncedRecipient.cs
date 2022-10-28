#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class BouncedRecipient
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("diagnosticCode")]
    public string DiagnosticCode { get; set; }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; set; }
}
