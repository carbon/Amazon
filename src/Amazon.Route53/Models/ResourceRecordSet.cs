#nullable disable

using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Amazon.Route53;

public sealed class ResourceRecordSet
{
    public ResourceRecordSet() { }

#nullable enable
    public ResourceRecordSet(ResourceRecordType type, string name, params ResourceRecord[] resourceRecords)
    {
        ArgumentNullException.ThrowIfNull(name);

        Type = type;
        Name = name;
        ResourceRecords = resourceRecords;
    }
#nullable disable

    public AliasTarget AliasTarget { get; init; }

    [DefaultValue(Failover.None)]
    public Failover Failover { get; init; }

    public GeoLocation GeoLocation { get; init; }

    public string HealthCheckId { get; init; }

    [DefaultValue(false)]
    public bool MultiValueAnswer { get; init; }

    public string Name { get; init; }

    public string Region { get; init; }

    [XmlArray("ResourceRecords")]
    [XmlArrayItem("ResourceRecord")]
    public ResourceRecord[] ResourceRecords { get; init; }

    public string SetIdentifier { get; init; }

    public string TrafficPolicyInstanceId { get; init; }

    [DefaultValue(0)]
    public int TTL { get; init; }

    public ResourceRecordType Type { get; init; }

    [DefaultValue(0)] // 0 & 255
    public byte Weight { get; init; }
}
