namespace Amazon.Ssm;

public sealed class UpdateDocumentDefaultVersionRequest : ISsmRequest
{
    public required string DocumentVersion { get; set; }

    public required string Name { get; set; }
}
