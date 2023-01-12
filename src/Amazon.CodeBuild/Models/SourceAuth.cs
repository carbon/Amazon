namespace Amazon.CodeBuild;

public sealed class SourceAuth
{
    public required string Type { get; set; }

    public string? Resource { get; set; }
}