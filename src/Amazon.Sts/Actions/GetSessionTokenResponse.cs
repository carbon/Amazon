#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetSessionTokenResponse : IStsResponse
{
    [XmlElement]
    public GetSessionTokenResult GetSessionTokenResult { get; init; }
}

public sealed class GetSessionTokenResult
{
    [XmlElement]
    public Credentials Credentials { get; init; }
}
