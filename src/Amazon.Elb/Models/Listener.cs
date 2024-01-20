#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class Listener
{
    [XmlElement]
    public string LoadBalancerArn { get; init; }

    [XmlElement]
    public Protocol Protocol { get; init; }

    [XmlElement]
    public int Port { get; init; }

    [XmlElement]
    public string ListenerArn { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public ListenerAction[] DefaultActions { get; init; }
}