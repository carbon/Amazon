#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models;

public readonly struct ResponseMetadata
{
    [XmlElement("RequestId")]
    public string RequestId { get; init; }
}