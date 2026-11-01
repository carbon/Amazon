using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleWithSamlResult
{
    [XmlElement]
    public required Credentials Credentials { get; init; }

    [XmlElement]
    public required AssumedRoleUser AssumedRoleUser { get; init; }

    [XmlElement]
    public string? Audience { get; init; }

    [XmlElement]
    public string? Issuer { get; init; }

    [XmlElement]
    public string? NameQualifier { get; init; }

    [XmlElement]
    public int PackedPolicySize { get; init; }

    [XmlElement]
    public string? SourceIdentity { get; init; }

    [XmlElement]
    public string? Subject { get; init; }

    [XmlElement]
    public string? SubjectType { get; init; }
}