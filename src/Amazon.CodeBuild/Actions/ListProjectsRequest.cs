namespace Amazon.CodeBuild
{
    public class ListProjectsRequest : ICodeBuildRequest
    {
        public string NextToken { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}