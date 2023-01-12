namespace Amazon.Ssm;

public sealed class CreateActivationRequest : ISsmRequest
{
    public string? DefaultInstanceName { get; init; }

    public string? Description { get; init; }

    public string? ExpirationDate { get; init; }

    public required string IamRole { get; init; }

    public int? RegistrationLimit { get; init; }
}
