#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class Error
{
    [XmlElement]
    public string Type { get; set; }

    [XmlElement]
    public string Code { get; set; }

    [XmlElement]
    public string Message { get; set; }
}