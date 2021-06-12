#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public sealed class ModifyLoadBalancerAttributesResponse : IElbResponse
    {
        [XmlElement]
        public ModifyLoadBalancerAttributesResult ModifyLoadBalancerAttributesResult { get; init; }
    }

    public sealed class ModifyLoadBalancerAttributesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public LoadBalancerAttribute[] Attributes { get; init; }
    }
}