using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public class DeregisterTargetsRequest : IElbRequest
    {
        public string Action => "DeregisterTargets";

        [Required]
        public string TargetGroupArn { get; set; }

        public List<TargetDescription> Targets { get; } = new List<TargetDescription>();
    }
}