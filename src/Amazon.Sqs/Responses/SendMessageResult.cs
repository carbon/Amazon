#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models;

public sealed class SendMessageResponse
{
    [XmlElement("SendMessageResult")]
    public SendMessageResult SendMessageResult { get; init; }

    public static SendMessageResponse Deserialize(string xmlText)
    {
        return SqsSerializer<SendMessageResponse>.Deserialize(xmlText);
    }
}

public sealed class SendMessageResult
{
    [XmlElement("MD5OfMessageBody")]
    public string MD5OfMessageBody { get; init; }

    [XmlElement("MD5OfMessageAttributes")]
    public string MD5OfMessageAttributes { get; init; }

    [XmlElement("MessageId")]
    public string MessageId { get; init; }
}

/* 
<SendMessageResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
    <SendMessageResult>
        <MD5OfMessageBody>5d41402abc4b2a76b9719d911017c592</MD5OfMessageBody>
        <MD5OfMessageAttributes>3ae8f24a165a8cedc005670c81a27295</MD5OfMessageAttributes>
        <MessageId>cafaea9a-70f8-47c7-89b3-7bbb572cf061</MessageId>
    </SendMessageResult>
</SendMessageResponse>
*/
