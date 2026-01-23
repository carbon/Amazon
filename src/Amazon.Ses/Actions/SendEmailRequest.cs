using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Ses;

public sealed class SendEmailRequest
{
    [JsonPropertyName("ConfigurationSetName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ConfigurationSetName { get; init; }

    [JsonPropertyName("Content")]
    public required EmailContent Content { get; init; }

    [JsonPropertyName("Destination")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Destination? Destination { get; init; }

    [JsonPropertyName("FromEmailAddress")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FromEmailAddress { get; init; }

    [JsonPropertyName("ReplyToAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ReplyToAddresses { get; init; }

    public static SendEmailRequest FromSesEmail(SesEmail r)
    {
        ArgumentNullException.ThrowIfNull(r);

        return new SendEmailRequest {
            Content = new EmailContent {
               Simple = new() {
                   Subject = r.Subject.Data,
                   Body = new MessageBody {
                       Html = r.Html != null ? new Content { Data = r.Html.Data, Charset = "UTF-8" } : null,
                       Text = r.Text != null ? new Content { Data = r.Text.Data, Charset = "UTF-8" } : null,
                   }
               }
            },
            FromEmailAddress = r.Source,
            Destination = new Destination {
                ToAddresses = r.To,
                CcAddresses = r.Cc,
                BccAddresses = r.Bcc
            },
            ReplyToAddresses = r.ReplyTo
        };
    }
}


public class SendEmailResult
{
    [JsonPropertyName("MessageId")]
    public required string MessageId { get; init; }
}

public class EmailContent
{
    [JsonPropertyName("Simple")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Message? Simple { get; set; }

    [JsonPropertyName("Raw")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RawMessage? Raw { get; set; }

    public static implicit operator EmailContent(RawMessage message)
    {
        return new EmailContent { Raw = message };
    }
}

public sealed class RawMessage
{
    public RawMessage() { }

    [SetsRequiredMembers]
    public RawMessage(byte[] data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Data = data;
    }

    [JsonPropertyName("Data")]
    public required byte[] Data { get; init; }
}


public class Destination
{
    [JsonPropertyName("BccAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? BccAddresses { get; init; }

    [JsonPropertyName("CcAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? CcAddresses { get; init; }

    [JsonPropertyName("ToAddresses")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? ToAddresses { get; init; }
}


public sealed class Message
{
    [JsonPropertyName("Body")]
    public required MessageBody Body { get; init; }

    [JsonPropertyName("Subject")]
    public required Content Subject { get; init; }

    [JsonPropertyName("Attachments")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<EmailAttachment>? Attachments { get; set; }

    [JsonPropertyName("Headers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<MessageHeader>? Headers { get; set; }
}

public readonly struct MessageHeader
{
    public MessageHeader() { }

    [SetsRequiredMembers]
    public MessageHeader(string name, string value)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(value);

        Name = name;
        Value = value;
    }

    [JsonPropertyName("Name")]
    public required string Name { get; init; }

    [JsonPropertyName("Value")]
    public required string Value { get; init; }
}

public class MessageBody
{
    [JsonPropertyName("Html")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Content? Html { get; set; }

    [JsonPropertyName("Text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Content? Text { get; set; }
}

public sealed class Content
{
    // UTF-8, ISO-8859-1, or Shift_JIS
    [JsonPropertyName("Charset")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Charset { get; init; }

    [JsonPropertyName("Data")]
    public required string Data { get; init; }

    public static implicit operator Content(string data)
    {
        return new Content { Charset = "UTF-8", Data = data };
    }
}

public class EmailAttachment
{

}