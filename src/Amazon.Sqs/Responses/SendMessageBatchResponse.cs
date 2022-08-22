#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageBatchResponse
{
    [XmlElement("SendMessageBatchResult")]
    public SendMessageBatchResult SendMessageBatchResult { get; init; }

    public static SendMessageBatchResponse Deserialize(string xmlText)
    {
        return SqsSerializer<SendMessageBatchResponse>.Deserialize(xmlText);
    }
}