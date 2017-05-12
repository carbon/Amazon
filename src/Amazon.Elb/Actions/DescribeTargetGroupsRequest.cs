namespace Amazon.Elb
{
    public class DescribeTargetGroupsRequest : IElbRequest
    {
        public string Action => "DescribeTargetGroups";

        public string LoadBalancerArn { get; set; }

        public string Marker { get; set; }

        public string[] Names { get; set; }

        public int? PageSize { get; set; }

        public string[] TargetGroupArns { get; set; }
    }
}