using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumedRoleUser
{
    [XmlElement]
    public required string Arn { get; init; }

    [XmlElement]
    public required string AssumedRoleId { get; init; }
}