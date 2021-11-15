#nullable disable

namespace Amazon.CodeBuild;

public sealed class ListBuildsResponse
{
    public string[] Ids { get; init; }

    public string NextToken { get; init; }
}
