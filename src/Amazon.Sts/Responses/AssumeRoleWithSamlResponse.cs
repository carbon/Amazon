#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleWithSamlResponse : IStsResponse
{
    [XmlElement] 
    public AssumeRoleResult AssumeRoleResult { get; set; }
}