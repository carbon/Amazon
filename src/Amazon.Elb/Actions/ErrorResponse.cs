#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class ErrorResponse
{
    [XmlElement]
    public Error Error { get; set; }
}