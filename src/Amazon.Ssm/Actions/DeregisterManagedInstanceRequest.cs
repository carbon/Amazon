using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DeregisterManagedInstanceRequest : ISsmRequest
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