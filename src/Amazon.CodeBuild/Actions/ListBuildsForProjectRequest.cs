using System.ComponentModel.DataAnnotations;

namespace Amazon.CodeBuild
{
    public class ListBuildsForProjectRequest : ICodeBuildRequest
    {
        public ListBuildsForProjectRequest() { }

        public ListBuildsForProjectRequest(string projectName)
        {
            ProjectName = projectName;
        }

        [Required]
        public string ProjectName { get; set; }

        // ASCENDING | DESCENDING
        public string SortOrder { get; set; }

        public string NextToken { get; set; }
    }
}