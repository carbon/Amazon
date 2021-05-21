#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public class SendMessageBatchResult
    {
        [XmlElement("SendMessageBatchResultEntry")]
        public SendMessageBatchResultEntry[] Items { get; set; }        
    }

    public class SendMessageBatchResultEntry
    {
        [XmlElement("Id")]
        public string Id { get; set; }

        [XmlElement("MessageId")]
        public string MessageId { get; set; }

        [XmlElement("MD5OfMessageBody")]
        public string MD5OfMessageBody { get; set; }
    }
}