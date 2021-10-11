#nullable disable

namespace Amazon.Ssm;

public sealed class DocumentVersionInfo
{
    public string Name { get; init; }

    public Timestamp CreatedDate { get; init; }

    public string DocumentVersion { get; init; }

    public bool IsDefaultVersion { get; init; }
}