#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class ListProjectsResponse
    {
        public string NextToken { get; init; }

        public string[] Projects { get; init; }
    }
}