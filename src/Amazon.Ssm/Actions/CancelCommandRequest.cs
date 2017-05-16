using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class CancelCommandRequest : ISsmRequest
    {
        [Required]
        public string CommandId { get; set; }

        // If empty, cancels all
        public string[] InstanceIds { get; set; }
    }
}