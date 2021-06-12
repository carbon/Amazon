#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class DeregisterTargetsRequest : IElbRequest
    {
        public DeregisterTargetsRequest() { }

        public DeregisterTargetsRequest(
            string targetGroupArn, 
            params TargetDescription[] targets)
        {
            TargetGroupArn = targetGroupArn ?? throw new ArgumentNullException(nameof(targetGroupArn));
            Targets        = targets        ?? throw new ArgumentNullException(nameof(targets));
        }

        public string Action => "DeregisterTargets";

        [Required]
        public string TargetGroupArn { get; init; }

        [Required]
        public TargetDescription[] Targets { get; init; }
    }
}