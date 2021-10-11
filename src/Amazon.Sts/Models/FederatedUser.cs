#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class FederatedUser
{
    [XmlElement]
    public string Arn { get; init; }

    [XmlElement]
    public string FederatedUserId { get; init; }
}
