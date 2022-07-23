namespace Amazon.CodeBuild;

public sealed class ListBuildsForProjectRequest : ICodeBuildRequest
{
    public ListBuildsForProjectRequest(
        string projectName,
        string? sortOrder = null,
        string? nextToken = null)
    {
        ArgumentNullException.ThrowIfNull(projectName);

        ProjectName = projectName;
        SortOrder = sortOrder;
        NextToken = nextToken;
    }

    public string ProjectName { get; }

    // ASCENDING | DESCENDING
    public string? SortOrder { get; }

    public string? NextToken { get; }
}