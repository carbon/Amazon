namespace Amazon.Elb
{
    public class DescribeTargetHealthRequest : IElbRequest
    {
        public string Action => "DescribeTargetHealth";
        
        public string TargetGroupArn { get; set; }

        public TargetDescription[] Targets { get; set; }
    }
}