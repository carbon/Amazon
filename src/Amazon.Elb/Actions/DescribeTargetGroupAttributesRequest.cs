using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DescribeTargetGroupAttributesRequest : IElbRequest
    {
        public string Action => "DescribeTargetGroupAttributes";

        [Required]
        public string TargetGroupArn { get; set; }
    }
}