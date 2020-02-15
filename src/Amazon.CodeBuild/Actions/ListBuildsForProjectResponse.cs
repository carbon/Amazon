#nullable disable

namespace Amazon.CodeBuild
{
    public class ListBuildsForProjectResponse
    {
        public string[] Ids { get; set; }

        public string NextToken { get; set; }
    }
}