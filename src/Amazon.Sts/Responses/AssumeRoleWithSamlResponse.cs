using System.Xml.Serialization;

namespace Amazon.Sts;

[XmlRoot("AssumeRoleWithSAMLResponse")]
public sealed class AssumeRoleWithSamlResponse : IStsResponse
{
    [XmlElement("AssumeRoleWithSAMLResult")]
    public required AssumeRoleWithSamlResult AssumeRoleWithSamlResult { get; init; }
}