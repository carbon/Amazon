namespace Amazon.Kms;

public sealed class KeyListEntry
{
    public required string KeyArn { get; init; }
    
    public required string KeyId { get; init; }
}