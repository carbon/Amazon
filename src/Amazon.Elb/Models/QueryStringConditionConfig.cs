using System.Xml.Serialization;

namespace Amazon.Elb;

public sealed class QueryStringConditionConfig
{
    [XmlArray]
    [XmlArrayItem("member")]
    public QueryStringKeyValuePair[]? Values { get; init; }
}
