#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class DecodeAuthorizationMessageResponse : IStsResponse
{
    [XmlElement]
    public string DecodedMessage { get; init; }
}