using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetFederationTokenResponse : IStsResponse
{
    [XmlElement]
    public required GetFederationTokenResult GetFederationTokenResult { get; init; }
}

public sealed class GetFederationTokenResult
{
    [XmlElement]
    public required Credentials Credentials { get; init; }

    [XmlElement]
    public required FederatedUser FederatedUser { get; init; }

    [XmlElement]
    public required int PackedPolicySize { get; init; }
}