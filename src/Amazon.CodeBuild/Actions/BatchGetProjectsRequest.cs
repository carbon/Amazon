namespace Amazon.CodeBuild;

public sealed class BatchGetProjectsRequest(params string[] names) : ICodeBuildRequest
{
    public string[] Names { get; } = names ?? throw new ArgumentNullException(nameof(names));
}