#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResponse : IStsResponse
{
    [XmlElement]
    public AssumeRoleResult AssumeRoleResult { get; init; }
}

public sealed class AssumeRoleResult
{
    [XmlElement]
    public Credentials Credentials { get; init; }

    [XmlElement]
    public AssumedRoleUser AssumedRoleUser { get; init; }
}