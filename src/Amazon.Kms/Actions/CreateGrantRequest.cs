namespace Amazon.Kms;

public sealed class CreateGrantRequest : KmsRequest
{
    public required string KeyId { get; init; }

    /// <summary>
    /// The principal that is given permission to perform the operations that the grant permits.
    /// </summary>
    public required string GranteePrincipal { get; init; }

    /// <summary>
    /// There may a slight delay for a grant created in AWS KMS to take effect throughout a region.
    /// If you need to mitigate this delay, a grant token is a type of identifier that is 
    /// designed to let the permissions in the grant take effect immediately.
    /// </summary>
    public string[]? GrantTokens { get; init; }

    /// <summary>
    /// A friendly name for identifying the grant. 
    /// Use this value to prevent unintended creation of duplicate grants when retrying this request.
    /// </summary>
    public string? Name { get; init; }

    public required string[] Operations { get; init; }

    public GrantConstraints? Constraints { get; init; }

    public string? RetiringPrincipal { get; init; }
}