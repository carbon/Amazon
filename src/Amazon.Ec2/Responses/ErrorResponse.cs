#nullable disable

using System.IO;
using System.Xml.Serialization;

using Amazon.Ec2.Models;

namespace Amazon.Ec2.Responses;

[XmlRoot("Response")]
public sealed class ErrorResponse
{
    [XmlArray("Errors")]
    [XmlArrayItem("Error")]
    public Error[] Errors { get; set; }

    [XmlElement("RequestID")]
    public string RequestId { get; set; }

    private static readonly XmlSerializer serializer = new(typeof(ErrorResponse));

    public static ErrorResponse Deserialize(string xmlText)
    {
        using var textReader = new StringReader(xmlText);

        return (ErrorResponse)serializer.Deserialize(textReader)!;
    }
}