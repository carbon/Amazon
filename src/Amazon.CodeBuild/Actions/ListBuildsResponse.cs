#nullable disable

namespace Amazon.CodeBuild
{
    public sealed class ListBuildsResponse
    {
        public string[] Ids { get; set; }

        public string NextToken { get; set; }
    }
}