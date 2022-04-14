namespace Amazon.CodeBuild;

public sealed class DeleteProjectRequest : ICodeBuildRequest
{
    public DeleteProjectRequest(string name!!)
    {
        Name = name;
    }

    public string Name { get; }
}