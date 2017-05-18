namespace Amazon.CodeBuild
{
    public class ListProjectsRequest : ICodeBuildRequest
    {
        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public string NextToken { get; set; }
    }
}