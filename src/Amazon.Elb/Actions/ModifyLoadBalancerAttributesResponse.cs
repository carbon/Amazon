using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class ModifyLoadBalancerAttributesResponse : IElbResponse
    {
        [XmlElement]
        public ModifyLoadBalancerAttributesResult ModifyLoadBalancerAttributesResult { get; set; }
    }

    public class ModifyLoadBalancerAttributesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public LoadBalancerAttribute[] Attributes { get; set; }
    }
}