#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs.Models;

public sealed class ErrorResponse
{
    [XmlElement("Error")]
    public SqsError Error { get; init; }

    [XmlElement("RequestId")]
    public string RequestId { get; init; }

    public static ErrorResponse Deserialize(string xmlText)
    {
        return SqsSerializer<ErrorResponse>.Deserialize(xmlText);
    }
}