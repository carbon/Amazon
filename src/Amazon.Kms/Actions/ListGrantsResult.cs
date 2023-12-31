namespace Amazon.Kms;

public sealed class ListGrantsResult : KmsResult
{
    public string? NextMarker { get; init; }

    public bool Truncated { get; init; }

    public required List<Grant> Grants { get; init; }
}