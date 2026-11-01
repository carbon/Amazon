using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class SourceIpConditionConfig
{
    // ipv4 | ipv6
    public string? IpAddressType { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[]? Values { get; init; }
}
