#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SesCommonHeaders
{
    [JsonPropertyName("messageId")]
    public string MessageId { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("to")]
    public string[] To { get; set; }

    [JsonPropertyName("cc")]
    public string[] Cc { get; set; }

    [JsonPropertyName("bcc")]
    public string[] Bcc { get; set; }

    [JsonPropertyName("from")]
    public string[] From { get; set; }

    [JsonPropertyName("sender")]
    public string Sender { get; set; }

    [JsonPropertyName("returnPath")]
    public string ReturnPath { get; set; }

    [JsonPropertyName("reply-to")]
    public string[] ReplyTo { get; set; }

    [JsonPropertyName("subject")]
    public string Subject { get; set; }
}