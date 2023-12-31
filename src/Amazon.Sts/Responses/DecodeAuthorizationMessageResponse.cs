using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class DecodeAuthorizationMessageResponse : IStsResponse
{
    [XmlElement]
    public required string DecodedMessage { get; init; }
}