#nullable disable

namespace Amazon.Ssm
{
    public sealed class DeleteAssociationRequest
    {
        public string AssociationId { get; set; }

        public string InstanceId { get; set; }

        public string Name { get; set; }
    }
}