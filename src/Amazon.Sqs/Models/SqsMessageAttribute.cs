#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs;

public sealed class SqsMessageAttribute
{
    [XmlElement("Name")]
    public string Name { get; init; }

    [XmlElement("Value")]
    public SqsMessageAttributeValue Value { get; init; }
}

// <MessageAttribute>
//   <Name>id2</Name>
//   <Value>
//     <StringValue>qra2</StringValue>
//     <DataType>String</DataType>
//   </Value>
// </MessageAttribute>