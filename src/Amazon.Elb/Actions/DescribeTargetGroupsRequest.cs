#nullable disable

namespace Amazon.Elb
{
    public sealed class DescribeTargetGroupsRequest : IElbRequest
    {
        public string Action => "DescribeTargetGroups";

        public string LoadBalancerArn { get; init; }

        public string Marker { get; init; }

        public string[] Names { get; init; }

        public int? PageSize { get; init; }

        public string[] TargetGroupArns { get; init; }
    }
}