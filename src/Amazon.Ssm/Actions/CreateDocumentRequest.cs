namespace Amazon.Ssm;

public sealed class CreateDocumentRequest : ISsmRequest
{
    public required string Content { get; set; }

    public DocumentType? DocumentType { get; set; }

    public string? Name { get; set; }
}