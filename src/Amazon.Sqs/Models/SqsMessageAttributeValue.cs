#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public sealed class SqsMessageAttributeValue
    {
        [XmlElement("StringValue")]
        public string StringValue { get; set; }

        [XmlElement("BinaryValue")]
        public string BinaryValue { get; set; }

        [XmlElement("DataType")]
        public string DataType { get; set; }
    }
}