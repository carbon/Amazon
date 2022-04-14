#pragma warning disable IDE0090 // Use 'new(...)'

#nullable disable

using System.Xml.Serialization;

using Carbon.Messaging;

namespace Amazon.Sqs;

public sealed class SqsMessage : IQueueMessage<string>
{
    public SqsMessage() { }

    public SqsMessage(string body!!)
    {
        Body = body;
    }

    [XmlElement("MessageId")]
    public string MessageId { get; set; }

    [XmlElement("ReceiptHandle")]
    public string ReceiptHandle { get; set; }

    [XmlElement("Body")]
    public string Body { get; set; }

    [XmlElement("SequenceNumber")]
    public string SequenceNumber { get; set; }

    // SentTimestamp
    // SenderId
    // ...
    [XmlElement("Attribute")]
    public SqsSystemMessageAttribute[] Attributes { get; set; }

    [XmlElement("MessageAttribute")]
    public SqsMessageAttribute[] MessageAttributes { get; set; }

    public DateTime Created { get; set; }

    public DateTime Expires { get; set; }

    // ApproximateReceiveCount
    // ApproximateFirstReceiveTimestamp
    // MessageDedublicationId
    // MessageGroupId
    // SenderId
    // SentTimestamp
    // SequenceNumber

    // TODO: Attributes

    #region IQueueMessage<string>

    string IQueueMessage<string>.Id => MessageId;

    MessageReceipt IQueueMessage<string>.Receipt => new MessageReceipt(ReceiptHandle);

    #endregion
}
