using System;
using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class LoadBalancer
    {
        [XmlElement]
        public string LoadBalancerArn { get; set; }

        [XmlElement]
        public string Scheme { get; set; }

        [XmlElement]
        public string LoadBalancerName { get; set; }

        [XmlElement]
        public string VpcId { get; set; }

        [XmlElement]
        public string CanonicalHostedZoneId { get; set; }

        [XmlElement]
        public DateTime CreateTime { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public AvailabilityZone[] AvailabilityZones { get; set; }

        [XmlArray]
        [XmlArrayItem("member")]
        public string[] SecurityGroups { get; set; }

        [XmlElement]
        public string DNSName { get; set; }

        [XmlElement]
        public LoadBalancerState State { get; set; }

        [XmlElement]
        public string Type { get; set; }
    }

    public class LoadBalancerState
    {
        //  active | provisioning | failed
        [XmlElement]
        public string Code { get; set; }

        [XmlElement]
        public string Reason { get; set; }
    }
}
