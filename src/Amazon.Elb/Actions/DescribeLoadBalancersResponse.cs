#nullable disable

using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeLoadBalancersResponse : IElbResponse
    {
        [XmlElement]
        public DescribeLoadBalancersResult DescribeLoadBalancersResult { get; set; }
    }

    public class DescribeLoadBalancersResult
    {
        [XmlArray]
        [XmlArrayItem("member")]
        public LoadBalancer[] LoadBalancers { get; set; }
    }
}