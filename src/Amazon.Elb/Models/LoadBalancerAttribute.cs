using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class LoadBalancerAttribute
{
    public LoadBalancerAttribute() { }

    [SetsRequiredMembers]
    public LoadBalancerAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }

    [XmlElement]
    public required string Key { get; init; }

    [XmlElement]
    public required string Value { get; init; }
}