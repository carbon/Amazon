#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageBatchResult
{
    [XmlElement("SendMessageBatchResultEntry")]
    public SendMessageBatchResultEntry[] Items { get; init; }
}

public sealed class SendMessageBatchResultEntry
{
    [XmlElement("Id")]
    public string Id { get; init; }

    [XmlElement("MessageId")]
    public string MessageId { get; init; }

    [XmlElement("MD5OfMessageBody")]
    public string MD5OfMessageBody { get; init; }
}