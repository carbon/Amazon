#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sqs;

public sealed class SqsError
{
    [XmlElement("Type")]
    public string Type { get; init; }

    [XmlElement("Code")]
    public string Code { get; init; }

    [XmlElement("Message")]
    public string Message { get; init; }

#nullable enable

    [XmlElement("detail")]
    public string? Detail { get; init; }
}
