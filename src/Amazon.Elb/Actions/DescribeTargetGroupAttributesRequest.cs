using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DescribeTargetGroupAttributesRequest : IElbRequest
    {
        public string Action => "DescribeTargetGroupAttributes";

        public DescribeTargetGroupAttributesRequest() { }

        public DescribeTargetGroupAttributesRequest(string targetGroupArn)
        {
            TargetGroupArn = targetGroupArn;
        }

        [Required]
        public string TargetGroupArn { get; set; }
    }
}