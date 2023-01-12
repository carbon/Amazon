namespace Amazon.CodeBuild;

public sealed class StopBuildRequest : ICodeBuildRequest
{
    public StopBuildRequest(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        Id = id;
    }

    public string Id { get; }
}