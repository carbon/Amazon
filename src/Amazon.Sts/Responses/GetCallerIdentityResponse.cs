#nullable disable

using System.Xml.Serialization;

namespace Amazon.Sts;

public sealed class GetCallerIdentityResponse : IStsResponse
{
    [XmlElement]
    public GetCallerIdentityResult GetCallerIdentityResult { get; init; }
}