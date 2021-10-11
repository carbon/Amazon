#nullable disable

namespace Amazon.Ssm;

public sealed class InstanceInformation
{
    public string ActivationId { get; init; }

    public string AgentVersion { get; init; }

    public string AssociationStatus { get; init; }

    public string ComputerName { get; init; }

    public string InstanceId { get; init; }

    public string IamRole { get; init; }

    public bool IsLatestVersion { get; init; }

    public Timestamp? LastAssociationExecutionDate { get; init; }

    public Timestamp? LastPingDateTime { get; init; }

    public Timestamp? LastSuccessfulAssociationExecutionDate { get; init; }

    public string Name { get; init; }

    public PingStatus PingStatus { get; init; }

    public string PlatformName { get; init; }

    public PlatformType PlatformType { get; init; }

    public string PlatformVersion { get; init; }

    public Timestamp? RegistrationDate { get; init; }

    // ManagedInstance | Document | EC2Instance
    public ResourceType ResourceType { get; set; }
}
