#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public sealed class GetHostedZoneResponse
    {
        [XmlElement]
        public DelegationSet DelegationSet { get; init; }

        [XmlElement]
        public HostedZone HostedZone { get; init; }

        [XmlArray]
        [XmlArrayItem("VPC")]
        public VPC[] VPCs { get; init; }
    }
}