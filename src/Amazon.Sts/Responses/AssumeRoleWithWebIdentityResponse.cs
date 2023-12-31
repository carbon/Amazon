using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleWithWebIdentityResponse : IStsResponse
{
    [XmlElement]
    public required AssumeRoleWithWebIdentityResult AssumeRoleWithWebIdentityResult { get; init; }
}
