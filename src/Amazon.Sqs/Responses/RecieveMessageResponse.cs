#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models;

public sealed class ReceiveMessageResponse
{
    [XmlElement("ReceiveMessageResult")]
    public ReceiveMessageResult ReceiveMessageResult { get; init; }

    public static ReceiveMessageResponse ParseXml(string xmlText)
    {
        return SqsSerializer<ReceiveMessageResponse>.Deserialize(xmlText);
    }
}

public sealed class ReceiveMessageResult
{
    [XmlElement("Message")]
    public SqsMessage[] Items { get; init; }
}