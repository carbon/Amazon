#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public readonly struct SesRecipient
{
    [JsonConstructor]
    public SesRecipient(string emailAddress!!)
    {
        EmailAddress = emailAddress;
    }

    [JsonPropertyName("emailAddress")]
    public string EmailAddress { get; }
}