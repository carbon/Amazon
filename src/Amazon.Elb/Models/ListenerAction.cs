#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class ListenerAction
{
    [XmlElement]
    public string Type { get; init; }

    [XmlElement]
    public string TargetGroupArn { get; init; }

    public FixedResponseActionConfig FixedResponseConfig { get; init; }

    public ForwardActionConfig ForwardConfig { get; init; }

    public int? Order { get; init; }

    public RedirectActionConfig RedirectConfig { get; init; }
}
