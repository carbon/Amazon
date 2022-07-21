namespace Amazon.CodeBuild;

public sealed class BatchGetBuildsRequest : ICodeBuildRequest
{
    public BatchGetBuildsRequest(params string[] ids)
    {
        ArgumentNullException.ThrowIfNull(ids);

        Ids = ids;
    }

    public string[] Ids { get; }
}