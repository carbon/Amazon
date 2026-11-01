#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class RuleCondition
{
    // host-header, path-pattern
    [XmlElement]
    public string Field { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[] Values { get; init; }

    [XmlArray]
    [XmlArrayItem("member")]
    public string[] RegexValues { get; init; }

    public HostHeaderConditionConfig HostHeaderConfig { get; init; }

    public HttpHeaderConditionConfig HttpHeaderConfig { get; init; }

    public HttpRequestMethodConditionConfig HttpRequestMethodConfig { get; init; }

    public PathPatternConditionConfig PathPatternConfig { get; init; }

    public QueryStringConditionConfig QueryStringConfig { get; init; }

    public SourceIpConditionConfig SourceIpConfig { get; init; }
}