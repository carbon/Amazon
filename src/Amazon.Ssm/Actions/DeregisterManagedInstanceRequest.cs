#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class DeregisterManagedInstanceRequest : ISsmRequest
    {
        public DeregisterManagedInstanceRequest() { }

        public DeregisterManagedInstanceRequest(string instanceId)
        {
            InstanceId = instanceId;
        }

        [Required]
        public string InstanceId { get; set; }
    }
}