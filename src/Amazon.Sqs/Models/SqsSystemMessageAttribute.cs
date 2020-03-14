#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public class SqsSystemMessageAttribute
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public string Value { get; set; }
    }
}