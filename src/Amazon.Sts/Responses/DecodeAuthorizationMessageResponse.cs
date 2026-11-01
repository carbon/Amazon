using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class DecodeAuthorizationMessageResponse : IStsResponse
{
    [XmlElement]
    public required DecodeAuthorizationMessageResult DecodeAuthorizationMessageResult { get; init; }
}

public sealed class DecodeAuthorizationMessageResult
{
    [XmlElement]
    public required string DecodedMessage { get; init; }
}