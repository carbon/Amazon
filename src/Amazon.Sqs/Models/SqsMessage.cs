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
    public string MessageId { get; set; }

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; set; }

    [JsonPropertyName("Body")]
    public string Body { get; set; }

#nullable enable

    [JsonPropertyName("MD5OfBody")]
    public string? MD5OfBody { get; set; }

    [JsonPropertyName("MD5OfMessageAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MD5OfMessageAttributes { get; set; }

    [JsonPropertyName("SequenceNumber")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string SequenceNumber { get; set; }

    [JsonPropertyName("Attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string> Attributes { get; set; }

    [JsonPropertyName("MessageAttribute")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, MessageAttributeValue> MessageAttributes { get; set; }

    [JsonIgnore]
    public DateTime Created { get; set; }

    [JsonIgnore]
    public DateTime Expires { get; set; }
  
    #region IQueueMessage<string>

    string IQueueMessage<string>.Id => MessageId;

    MessageReceipt IQueueMessage<string>.Receipt => new MessageReceipt(ReceiptHandle);

    #endregion
}