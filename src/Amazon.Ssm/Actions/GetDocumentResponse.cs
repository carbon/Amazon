#nullable disable

namespace Amazon.Ssm;

public sealed class GetDocumentResponse
{
    public string Content { get; init; }

    public string DocumentType { get; init; }

    public string DocumentVersion { get; init; }

    public string Name { get; init; }
}
