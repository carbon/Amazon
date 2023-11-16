#nullable disable

using System.Text.Json.Serialization;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsMessage : IQueueMessage<string>
{
    public SqsMessage() { }

    public SqsMessage(string body)
    {
        ArgumentNullException.ThrowIfNull(body);

        Body = body;
    }

    [JsonPropertyName("MessageId")]
    public string MessageId { get; init; }

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; init; }

    [JsonPropertyName("Body")]
    public string Body { get; init; }

#nullable enable

    [JsonPropertyName("MD5OfBody")]
    public string? MD5OfBody { get; init; }

    [JsonPropertyName("MD5OfMessageAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MD5OfMessageAttributes { get; init; }

    [JsonPropertyName("SequenceNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string SequenceNumber { get; init; }

    [JsonPropertyName("Attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string> Attributes { get; init; }

    [JsonPropertyName("MessageAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, MessageAttributeValue> MessageAttributes { get; init; }
  
    #region IQueueMessage<string>

    string IQueueMessage<string>.Id => MessageId;

    MessageReceipt IQueueMessage<string>.Receipt => new MessageReceipt(ReceiptHandle);

    #endregion
}