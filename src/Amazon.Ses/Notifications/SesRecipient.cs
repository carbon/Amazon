#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public readonly struct SesRecipient
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; init; }
}