#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models
{
    public sealed class ResponseMetadata
    {
        [XmlElement("RequestId")]
        public string RequestId { get; set; }
    }
}