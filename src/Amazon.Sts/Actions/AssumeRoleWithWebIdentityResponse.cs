#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class AssumeRoleWithWebIdentityResponse : IStsResponse
{
    [XmlElement]
    public AssumeRoleWithWebIdentityResult AssumeRoleWithWebIdentityResult { get; init; }
}

public class AssumeRoleWithWebIdentityResult
{
    [XmlElement]
    public string SubjectFromWebIdentityToken { get; init; }

    [XmlElement]
    public string Audience { get; init; }

    [XmlElement]
    public Credentials Credentials { get; init; }

    [XmlElement]
    public string Provider { get; init; }
}