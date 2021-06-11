#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class ListBuildsForProjectResponse
    {
        public string[] Ids { get; init; }

        public string NextToken { get; init; }
    }
}