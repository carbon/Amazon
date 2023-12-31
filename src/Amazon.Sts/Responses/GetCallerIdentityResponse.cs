using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetCallerIdentityResponse : IStsResponse
{
    [XmlElement]
    public required GetCallerIdentityResult GetCallerIdentityResult { get; init; }
}