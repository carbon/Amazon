using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleWithSamlResponse : IStsResponse
{
    [XmlElement] 
    public required AssumeRoleResult AssumeRoleResult { get; init; }
}