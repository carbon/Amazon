namespace Amazon.CodeBuild;

public sealed class DeleteProjectRequest : ICodeBuildRequest
{
    public DeleteProjectRequest(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        Name = name;
    }

    public string Name { get; }
}