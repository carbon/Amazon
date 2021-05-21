#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    // <MessageAttribute>
    //   <Name>id2</Name>
    //   <Value>
    //     <StringValue>qra2</StringValue>
    //     <DataType>String</DataType>
    //   </Value>
    // </MessageAttribute>

    public sealed class SqsMessageAttribute
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public SqsMessageAttributeValue Value { get; set; }
    }
}