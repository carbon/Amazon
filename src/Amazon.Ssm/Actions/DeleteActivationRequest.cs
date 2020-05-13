#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Amazon.Ssm
{
    public sealed class DeleteActivationRequest : ISsmRequest
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