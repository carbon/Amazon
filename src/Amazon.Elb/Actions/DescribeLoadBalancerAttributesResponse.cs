using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeLoadBalancerAttributesResponse : IElbResponse
    {
        [XmlElement]
        public DescribeLoadBalancerAttributesResult DescribeLoadBalancerAttributesResult { get; set; }
    }

    public class DescribeLoadBalancerAttributesResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public LoadBalancerAttribute[] Attributes { get; set; }
    }
}
