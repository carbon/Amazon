#nullable disable

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Amazon.Route53
{
    public class ResourceRecordSet
    {
        public ResourceRecordSet() { }

#nullable enable
        public ResourceRecordSet(ResourceRecordType type, string name, params ResourceRecord[] resourceRecords)
        {
            Type            = type;
            Name            = name ?? throw new ArgumentNullException(nameof(name));
            ResourceRecords = resourceRecords;
        }
#nullable disable

        public AliasTarget AliasTarget { get; set; }

        [DefaultValue(Failover.None)]
        public Failover Failover { get; set; }

        public GeoLocation GeoLocation { get; set; }

        public string HealthCheckId { get; set; }

        [DefaultValue(false)]
        public bool MultiValueAnswer { get; set; }

        public string Name { get; set; }

        public string Region { get; set; }

        [XmlArray("ResourceRecords")]
        [XmlArrayItem("ResourceRecord")]
        public ResourceRecord[] ResourceRecords { get; set; }

        public string SetIdentifier { get; set; }

        public string TrafficPolicyInstanceId { get; set; }

        [DefaultValue(0)]
        public int TTL { get; set; }

        public ResourceRecordType Type { get; set; }

        [DefaultValue(0)] // 0 & 255
        public byte Weight { get; set; }
    }
}
