using System.Xml.Serialization;

namespace Amazon.Route53;

public sealed class DelegationSet
{
    [XmlArray]
    [XmlArrayItem("NameServer")]
    public required string[] NameServers { get; init; }

    public string? CallerReference { get; init; }

    public string? Id { get; init; }
}