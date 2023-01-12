#nullable disable

namespace Amazon.Kms;

public sealed class CreateGrantResponse : KmsResponse
{
    public string GrantId { get; init; }

    public string GrantToken { get; init; }
}