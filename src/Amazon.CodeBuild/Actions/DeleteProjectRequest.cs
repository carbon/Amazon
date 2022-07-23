namespace Amazon.CodeBuild;

public sealed class DeleteProjectRequest : ICodeBuildRequest
{
    public DeleteProjectRequest(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    public string Name { get; }
}