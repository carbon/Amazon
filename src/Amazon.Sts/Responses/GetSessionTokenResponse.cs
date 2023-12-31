using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetSessionTokenResponse : IStsResponse
{
    [XmlElement]
    public required GetSessionTokenResult GetSessionTokenResult { get; init; }
}

public sealed class GetSessionTokenResult
{
    [XmlElement]
    public required Credentials Credentials { get; init; }
}