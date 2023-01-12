namespace Amazon.Ssm;

public sealed class DescribeDocumentRequest : ISsmRequest
{
    public string? DocumentVersion { get; init; }

    public required string Name { get; init; }
}
