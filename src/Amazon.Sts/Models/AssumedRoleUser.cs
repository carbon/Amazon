#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumedRoleUser
{
    [XmlElement]
    public string Arn { get; init; }

    [XmlElement]
    public string AssumedRoleId { get; init; }
}