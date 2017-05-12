using System.Xml.Serialization;

namespace Amazon.Elb
{
    public class DescribeLoadBalancersRequest : IElbRequest
    {
        public string Action => "DescribeLoadBalancers";

        public string[] LoadBalancerArns { get; set; }

        public string Marker { get; set; }

        public string[] Names { get; set; }

        public int? PageSize { get; set; }

    }
}
