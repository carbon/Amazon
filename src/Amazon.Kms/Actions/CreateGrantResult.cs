namespace Amazon.Kms;

public sealed class CreateGrantResult : KmsResult
{
    public required string GrantId { get; init; }

    public required string GrantToken { get; init; }
}