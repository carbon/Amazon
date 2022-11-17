#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetCallerIdentityResult
{
    [XmlElement]
    public string Arn { get; init; }

    [XmlElement]
    public string UserId { get; init; }

    [XmlElement]
    public string Account { get; init; }
}