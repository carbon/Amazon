#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class SetIpAddressTypeResponse : IElbResponse
{
    [XmlElement]
    public SetIpAddressTypeResult SetIpAddressTypeResult { get; init; }
}

public sealed class SetIpAddressTypeResult
{
    [XmlElement]
    public string IpAddressType { get; init; }
}
