using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public class DeleteActivationRequest : ISsmRequest
    {
        public DeleteActivationRequest() { }

        public DeleteActivationRequest(string activationId)
        {
            ActivationId = activationId;
        }

        [Required]
        public string ActivationId { get; set; }
    }
}