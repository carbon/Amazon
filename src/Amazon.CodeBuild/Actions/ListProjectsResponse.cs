#nullable disable

namespace Amazon.CodeBuild
{
    public class ListProjectsResponse
    {
        public string NextToken { get; set; }

        public string[] Projects { get; set; }
    }
}