#nullable disable

using System.Xml.Serialization;

namespace Amazon.Ec2.Models;

public sealed class Error
{
    [XmlElement("Code")]
    public string Code { get; set; }

    [XmlElement("Message")]
    public string Message { get; set; }
}