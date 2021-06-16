#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public sealed class SqsMessageAttributeValue
    {
        [XmlElement("StringValue")]
        public string StringValue { get; init; }

        [XmlElement("BinaryValue")]
        public string BinaryValue { get; init; }

        [XmlElement("DataType")]
        public string DataType { get; init; }
    }
}