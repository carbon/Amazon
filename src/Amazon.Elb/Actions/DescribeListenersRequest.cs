#nullable disable

namespace Amazon.Elb
{
    public sealed class DescribeListenersRequest : IElbRequest
    {
        public string Action => "DescribeListeners";
        
        public string[] ListenerArns { get; set; }

        public string LoadBalancerArn { get; set; }

        public string Marker { get; set; }

        public int? PageSize { get; set; }
    }
}
