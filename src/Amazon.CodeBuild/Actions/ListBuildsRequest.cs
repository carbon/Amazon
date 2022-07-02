#nullable disable

namespace Amazon.CodeBuild;

public class ListBuildsRequest : ICodeBuildRequest
{
    public string NextToken { get; set; }

    public string SortOrder { get; set; }
}