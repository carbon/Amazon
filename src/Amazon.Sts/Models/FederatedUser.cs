using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class FederatedUser
{
    [XmlElement]
    public required string Arn { get; init; }

    [XmlElement]
    public required string FederatedUserId { get; init; }
}
