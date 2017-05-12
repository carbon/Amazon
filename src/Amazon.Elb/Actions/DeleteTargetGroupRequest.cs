using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DeleteTargetGroupRequest : IElbRequest
    {
        public string Action => "DeleteTargetGroup";

        [Required]
        public string TargetGroupArn { get; set; }
    }
}
