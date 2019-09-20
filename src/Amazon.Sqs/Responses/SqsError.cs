#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public class SqsError
    {
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

        [XmlElement("detail")]
        public string Detail { get; set; }
    }
}