namespace Amazon.Ssm;

public sealed class GetDocumentRequest : ISsmRequest
{
    public string? DocumentVersion { get; init; }

    public required string Name { get; init; }
}