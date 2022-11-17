#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetFederationTokenResponse : IStsResponse
{
    [XmlElement]
    public GetFederationTokenResult GetFederationTokenResult { get; init; }
}

public sealed class GetFederationTokenResult
{
    [XmlElement]
    public Credentials Credentials { get; init; }

    [XmlElement]
    public FederatedUser FederatedUser { get; init; }

    [XmlElement]
    public int PackedPolicySize { get; init; }
}