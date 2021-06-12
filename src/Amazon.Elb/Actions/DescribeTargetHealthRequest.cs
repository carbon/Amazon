#nullable disable

namespace Amazon.Elb
{
    public sealed class DescribeTargetHealthRequest : IElbRequest
    {
        public string Action => "DescribeTargetHealth";
        
        public string TargetGroupArn { get; init; }

        public TargetDescription[] Targets { get; init; }
    }
}