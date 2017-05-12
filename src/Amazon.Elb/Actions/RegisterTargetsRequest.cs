using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class RegisterTargetsRequest : IElbRequest
    {
        public string Action => "RegisterTargets";

        [Required]
        public string TargetGroupArn { get; set; }

        public List<TargetDescription> Targets { get; } = new List<TargetDescription>();
    }
}