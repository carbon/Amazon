#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs
{
    public sealed class SqsError
    {
        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlElement("Code")]
        public string Code { get; set; }

        [XmlElement("Message")]
        public string Message { get; set; }

#nullable enable

        [XmlElement("detail")]
        public string? Detail { get; set; }
    }
}