using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public class SendMessageResponse
    {
        [XmlElement("SendMessageResult")]
        public SendMessageResult SendMessageResult { get; set; }

        public static SendMessageResponse Parse(string xmlText)
        {
            return SqsSerializer<SendMessageResponse>.Deserialize(xmlText);
        }
    }

    public class SendMessageResult
    {
        [XmlElement("MD5OfMessageBody")]
        public string MD5OfMessageBody { get; set; }

        [XmlElement("MessageId")]
        public string MessageId { get; set; }
    }
}

/* 
<SendMessageResponse xmlns="http://queue.amazonaws.com/doc/2009-02-01/">
    <SendMessageResult>
        <MD5OfMessageBody>5d41402abc4b2a76b9719d911017c592</MD5OfMessageBody>
        <MessageId>cafaea9a-70f8-47c7-89b3-7bbb572cf061</MessageId>
    </SendMessageResult>
</SendMessageResponse>
*/
