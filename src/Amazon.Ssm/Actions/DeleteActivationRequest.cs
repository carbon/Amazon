namespace Amazon.Ssm;

public sealed class DeleteActivationRequest : ISsmRequest
{
    public DeleteActivationRequest() { }

    public DeleteActivationRequest(string activationId)
    {
        ArgumentException.ThrowIfNullOrEmpty(activationId);

        ActivationId = activationId;
    }

    public required string ActivationId { get; init; }
}