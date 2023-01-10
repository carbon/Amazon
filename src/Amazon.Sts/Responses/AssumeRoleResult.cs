using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResult
{
    [XmlElement]
    public string? Issuer { get; init; }

    [XmlElement]
    public string? SourceIdentity { get; init; }

#nullable disable

    [XmlElement]
    public Credentials Credentials { get; init; }

    [XmlElement]
    public AssumedRoleUser AssumedRoleUser { get; init; }

    [XmlElement]
    public int PackedPolicySize { get; init; }
}