#nullable disable

using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class HostedZoneConfig
    {
        public string Comment { get; set; }

        public bool PrivateZone { get; set; }
    }

    public class VPC
    {
        public string VPCId { get; set; }

        public string VPCRegion { get; set; }
    }

}
