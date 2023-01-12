namespace Amazon.Ssm;

public sealed class ModifyDocumentPermissionRequest : ISsmRequest
{
    public string[]? AccountIdsToAdd { get; init; }

    public string[]? AccountIdsToRemove { get; init; }

    public required string Name { get; init; }

    public required string PermissionType { get; init; }
}