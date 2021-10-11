#nullable disable

namespace Amazon.Ssm;

public sealed class InstanceAssociation
{
    public string AssociationId { get; set; }

    public string Content { get; set; }

    public string InstanceId { get; set; }
}
