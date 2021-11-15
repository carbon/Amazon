namespace Amazon.CodeBuild;

public sealed class ListProjectsRequest : ICodeBuildRequest
{
    // NAME | CREATED_TIME | LAST_MODIFIED_TIME
    public string? SortBy { get; set; }

    // ASCENDING | DESCENDING
    public string? SortOrder { get; set; }

    public string? NextToken { get; set; }
}
