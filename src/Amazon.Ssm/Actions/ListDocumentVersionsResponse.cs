#nullable disable

namespace Amazon.Ssm;

public sealed class ListDocumentVersionsResponse
{
    public DocumentVersionInfo[] DocumentVersions { get; set; }

    public string NextToken { get; set; }
}
