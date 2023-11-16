#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

[method: JsonConstructor]
public readonly struct SesRecipient(string emailAddress)
{
    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; } = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
}