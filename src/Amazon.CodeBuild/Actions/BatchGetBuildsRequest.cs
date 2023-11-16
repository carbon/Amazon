namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsRequest(params string[] ids) : ICodeBuildRequest
{
    public string[] Ids { get; } = ids ?? throw new ArgumentNullException(nameof(ids));
}