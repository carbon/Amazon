#nullable disable

namespace Amazon.Kms;

public sealed class ListGrantsResponse : KmsResponse
{
    public string NextMarker { get; init; }

    public bool Truncated { get; init; }

    public List<Grant> Grants { get; init; }
}