using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResponse : IStsResponse
{
    [XmlElement]
    public required AssumeRoleResult AssumeRoleResult { get; init; }
}