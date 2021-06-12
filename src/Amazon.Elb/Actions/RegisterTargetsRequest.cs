#nullable disable

using System;
using System.ComponentModel.DataAnnotations;

namespace Amazon.Elb
{
    public sealed class RegisterTargetsRequest : IElbRequest
    {
        public RegisterTargetsRequest() { }

        public RegisterTargetsRequest(string targetGroupArn, params TargetDescription[] targets)
        {
            TargetGroupArn = targetGroupArn ?? throw new ArgumentNullException(nameof(targetGroupArn));
            Targets        = targets        ?? throw new ArgumentNullException(nameof(targets));
        }

        public string Action => "RegisterTargets";

        [Required]
        public string TargetGroupArn { get; init; }

        [Required]
        public TargetDescription[] Targets { get; init; }
    }
}