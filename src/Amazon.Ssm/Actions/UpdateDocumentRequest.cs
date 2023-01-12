namespace Amazon.Ssm;

public sealed class UpdateDocumentRequest : ISsmRequest
{
    public required string Content { get; init; }

    public string? DocumentVersion { get; init; }

    public required string Name { get; init; }
}
