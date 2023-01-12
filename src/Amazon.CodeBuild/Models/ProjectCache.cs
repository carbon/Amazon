namespace Amazon.CodeBuild;

public sealed class ProjectCache
{
    // NO_CACHE | S3
    public required ProjectCacheType Type { get; init; }

    public string? Location { get; init; }
}
