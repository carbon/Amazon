#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleResponse : IStsResponse
{
    [XmlElement]
    public AssumeRoleResult AssumeRoleResult { get; init; }
}