#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class GetHostedZoneResponse
    {
        [XmlElement]
        public DelegationSet DelegationSet { get; set; }

        [XmlElement]
        public HostedZone HostedZone { get; set; }

        [XmlArray]
        [XmlArrayItem("VPC")]
        public VPC[] VPCs { get; set; }
    }


    public class GetChangeResponse
    {
        public ChangeInfo ChangeInfo { get; set; }
    }


}