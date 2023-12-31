using System.Xml.Serialization;

namespace Amazon.Sts;

public class AssumeRoleWithWebIdentityResult
{
    [XmlElement]
    public required string SubjectFromWebIdentityToken { get; init; }

    [XmlElement]
    public required AssumedRoleUser AssumedRoleUser { get; init; }

    [XmlElement]
    public required string Audience { get; init; }


    [XmlElement]
    public required Credentials Credentials { get; init; }

    /// <summary>
    /// For OpenID Connect ID tokens, this contains the value of the iss field. 
    /// For OAuth 2.0 access tokens, this contains the value of the ProviderId parameter that was passed in the AssumeRoleWithWebIdentity request.
    /// </summary>
    [XmlElement]
    public required string Provider { get; init; }
}