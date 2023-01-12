namespace Amazon.CodeBuild;

public sealed class ListBuildsRequest : ICodeBuildRequest
{
    public string? NextToken { get; set; }

    public string? SortOrder { get; set; }
}