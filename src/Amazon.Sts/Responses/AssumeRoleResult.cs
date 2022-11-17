using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResult
{
    [XmlElement]
    public string? Issuer { get; set; }

#nullable disable

    [XmlElement]
    public Credentials Credentials { get; init; }

    [XmlElement]
    public AssumedRoleUser AssumedRoleUser { get; init; }
}